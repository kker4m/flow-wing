using FlowWing.Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider; // IServiceProvider eklendi

    public LoggingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider; // IServiceProvider ataması yapıldı
    }

    public async Task Invoke(HttpContext context)
    {
        // İstek bilgilerini al
        var request = context.Request;
        var requestBody = await FormatRequest(request);
        var userEmail = context.Items["UserEmail"]?.ToString();
        if (userEmail == null)
        {
            userEmail = "Anonymous";
        }
        // IP adresini al
        var ip = context.Connection.RemoteIpAddress.ToString();

        // İstek bilgilerini log'a yaz
        var logMessage = $"Request Method: {request.Method}, Path: {request.Path}, Body: {requestBody}, Email: {userEmail}, IP Address: {ip}";
        await LogMessageAsync(logMessage);

        // Middleware zincirini devam ettir
        await _next(context);

        // Yanıt bilgilerini al
        var response = context.Response;
        var originalBodyStream = context.Response.Body;

        // Yanıt bilgilerini log'a yaz
        logMessage = $"Response Status: {response.StatusCode}, Body: {originalBodyStream}";
        await LogMessageAsync(logMessage);
    }

    private async Task<string> FormatRequest(HttpRequest request)
    {
        var body = request.Body;

        // Request body'si okunabilir olmalı, bu yüzden baştan başlat
        request.EnableBuffering();

        // Request body'si oku
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        var bodyAsText = Encoding.UTF8.GetString(buffer);

        // Request body'sini geri yükle
        request.Body.Seek(0, SeekOrigin.Begin);

        return bodyAsText;
    }


    private async Task LogMessageAsync(string message)
    {
        //serviceProvider ile ILogger servisini al logu veritabanina kaydet
        using (var scope = _serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILoggingService>();
            await logger.CreateLogAsync(message);
        }
    }
}
