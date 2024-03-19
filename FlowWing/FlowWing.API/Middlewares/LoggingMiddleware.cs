using FlowWing.Business.Abstract;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggingService _loggingService;

    public LoggingMiddleware(RequestDelegate next, ILoggingService loggingService)
    {
        _next = next;
        _loggingService = loggingService;
    }

    public async Task Invoke(HttpContext context)
    {
        await _loggingService.CreateLogAsync($"Request: {context.Request.Method} {context.Request.Path}");
        await _next(context);
        await _loggingService.CreateLogAsync($"Response: {context.Response.StatusCode}");
    }
}
