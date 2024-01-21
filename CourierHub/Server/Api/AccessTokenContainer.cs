using CourierHub.Shared.ApiModels;

namespace CourierHub.Server.Api {
    public class AccessTokenContainer {
        private class AccessTokenResponse {
            public string? access_token { get; set; }

            // seems to be number of seconds untill the token is expired
            public string? token_type { get; set; }
            public int expires_in { get; set; }
        }

        private static readonly Dictionary<string, (string token, DateTime expiration)> tokens = new();

        public bool IsServiceTokenCachedAndNotExpired(ApiService service) {
            return tokens.TryGetValue(service.Name, out var tokenData) && DateTime.Now < tokenData.expiration;
        }

        public string? GetToken(ApiService service, string clientId, string clientSecret, string baseAddress, string tokenEndPoint) {
            if (tokens.TryGetValue(service.Name, out var tokenData)) {
                if (DateTime.Compare(DateTime.Now, tokenData.expiration) > 0) {
                    return tokenData.token;
                } else {
                    var tokenResponse = GetAccessToken(clientId, clientSecret, baseAddress, tokenEndPoint).Result;
                    if (tokenResponse != null && tokenResponse.access_token != null) {
                        tokens[service.Name] = (tokenResponse.access_token, DateTime.Now.AddMinutes(tokenResponse.expires_in));
                        return tokenResponse.access_token;
                    }
                }
            } else {
                var tokenResponse = GetAccessToken(clientId, clientSecret, baseAddress, tokenEndPoint).Result;
                if (tokenResponse != null && tokenResponse.access_token != null) {
                    tokens.Add(service.Name, (tokenResponse.access_token, DateTime.Now.AddMinutes(tokenResponse.expires_in)));
                    return tokenResponse.access_token;
                }
            }
            return null;
        }

        private static async Task<AccessTokenResponse?> GetAccessToken(string clientId, string clientSecret, string baseAddress, string tokenEndpoint) {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
            var credentials = $"{clientId}:{clientSecret}";
            var base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);

            var formData = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

            var response = await client.PostAsync($"{tokenEndpoint}", formData);
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
            } else {
                return null;
            }
        }
    }
}
