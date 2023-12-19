using CourierHubWebApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CourierHubWebApi.Middleware {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyMiddleware {
        private readonly RequestDelegate _next;
        private static readonly string _apiKeyName = "X-Api-Key";
        private static readonly string _serviceId = "ServiceIdIndex";
        public ApiKeyMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext context, [FromServices] IApiKeyService apiKeyService) {
            if (!context.Request.Headers.TryGetValue(_apiKeyName, out StringValues extractedApiKey)) {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Api Key was not provided");
            }
            string? serviceIdIndex = context.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>(_serviceId);
            string? key = extractedApiKey.First();
            if (key == null || !apiKeyService.TryGetServiceId(key, out int serviceId)) {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Unauthorized client");
            }

            if (serviceIdIndex != null)
                context.Items[serviceIdIndex] = serviceId.ToString();

            return _next(context);

        }
        public static async Task<string> GetRequestBody(HttpContext context) {
            var bodyStream = new StreamReader(context.Request.Body);
            var bodyText = await bodyStream.ReadToEndAsync();
            return bodyText;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }

}
