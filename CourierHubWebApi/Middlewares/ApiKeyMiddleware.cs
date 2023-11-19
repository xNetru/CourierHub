using Microsoft.Extensions.Primitives;

namespace CourierHubWebApi.Middleware {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyMiddleware {
        private readonly RequestDelegate _next;
        private const string _apiKeyName = "ApiKey"; // to prawdopodobnie będzie należało załadować z configa
        public ApiKeyMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext context) {
            if (!context.Request.Headers.TryGetValue(_apiKeyName, out StringValues extractedApiKey)) {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Api Key was not provided");
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            string? key = appSettings.GetValue<string>(_apiKeyName);
            if (key == null || !key.Equals(extractedApiKey)) {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Unauthorized client");
            }
            return _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
