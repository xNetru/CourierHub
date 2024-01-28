using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourierHub.Shared.Logging.Contracts;
using Microsoft.Extensions.Azure;
using CourierHubWebApi.Middlewares;
using Azure.Storage.Blobs.Models;
using CourierHubWebApi.Extensions;

namespace CourierHubWebApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext context, [FromServices] IMyLogger logger)
        {
            Stopwatch timer = Stopwatch.StartNew();
            Task result = _next(context);
            timer.Stop();
            logger.CreateLog(context, (ulong)timer.ElapsedMilliseconds);
            logger.SaveLog();
            return result;
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
