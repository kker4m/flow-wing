using FlowWing.Business.Abstract;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider; // IServiceProvider eklendi

    public LoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider; // IServiceProvider ataması yapıldı
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // ILoggingService'i scope olarak alın
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
                await loggingService.CreateLogAsync($"Request URL: {context.Request.Path}");
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            // Hata loglamayı yapın
            using (var scope = _serviceProvider.CreateScope())
            {
                var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
                await loggingService.CreateLogAsync($"Exception: {ex.Message}");
            }

            throw;
        }
    }
}