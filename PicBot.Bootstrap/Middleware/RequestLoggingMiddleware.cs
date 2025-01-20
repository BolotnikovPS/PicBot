using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PicBot.Bootstrap.Middleware;

public sealed class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger, RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var check = !httpContext.Request.Path.HasValue
                    || !httpContext.Request.Path.Value!.Contains("healthz");
        if (check)
        {
            logger.LogInformation($"Поступил запрос: {httpContext.Request.Method} {httpContext.Request.Path}");
        }

        await next(httpContext);

        if (check)
        {
            logger.LogInformation($"Запрос обработан ({httpContext.Request.Method} {httpContext.Request.Path})");
        }
    }
}