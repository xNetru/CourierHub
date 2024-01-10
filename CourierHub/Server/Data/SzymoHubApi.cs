using CourierHub.CourierHubApiModels;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using CourierHub.Shared.SzymoApiModels;

namespace CourierHub.Server.Data;
public class SzymoHubApi : IWebApi {
    private readonly ApiService _service;

    public string ServiceName { get; set; }

    public SzymoHubApi(ApiService service) {
        _service = service;
        ServiceName = _service.Name;
    }

    public async Task<(StatusType?, int)> GetOrderStatus(string code) {
    

        Console.WriteLine("GetOrderStatus was invoked in SzymoHubApi.");
        return (null, 400);
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        var source = new SzymoAddress(
            inquire.Source.Number,
            inquire.Source.Flat,
            inquire.Source.Street,
            inquire.Source.City,
            inquire.Source.PostalCode,
            "Poland");

        var destination = new SzymoAddress(
            inquire.Destination.Number,
            inquire.Destination.Flat,
            inquire.Destination.Street,
            inquire.Destination.City,
            inquire.Destination.PostalCode,
            "Poland");

        var dimensions = new SzymoDimensions(
            inquire.Width / 100.0f,
            inquire.Length / 100.0f,
            inquire.Depth / 100.0f,
            "Meters");

        var szymoInquiry = new SzymoInquiry(
            dimensions,
            "Pln",
            inquire.Mass / 1000.0f,
            "Kilograms",
            source,
            destination,
            inquire.SourceDate,
            inquire.DestinationDate,
            inquire.IsWeekend,
            inquire.Priority switch
            {
                (int)PriorityType.High => "High",
                (int)PriorityType.Low => "Low",
                (int)PriorityType.Medium => "Medium",
                _ => "Unknown"
            }, 
            true,
            inquire.IsCompany);



        Console.WriteLine("PostInquireGetOffer was invoked in SzymoHubApi.");
        return (null, 400);
    }

    public async Task<int> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in SzymoHubApi.");
        return 400;
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in SzymoHubApi.");
        return 400;
    }

    static async Task<string?> GetAccessToken(string clientId, string clientSecret, string tokenEndpoint)
    {
        using (HttpClient client = new HttpClient())
        {
            var credentials = $"{clientId}:{clientSecret}";
            var base64Credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
            });

            var response = await client.PostAsync(tokenEndpoint, formData);
            if(response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
                if(tokenResponse != null) 
                {
                    return tokenResponse.access_token;
                } 
                else
                {
                    return null;
                }
            } 
            else
            {
                return null;
            }
        }
    }

    static async Task MakeApiRequest(string apiUrl, string accessToken)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
        }
    }
}

class AccessTokenResponse
{
    public string? access_token { get; set; }
}
