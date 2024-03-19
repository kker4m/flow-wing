using FlowWing.API.Helpers;
using FlowWing.Business.Abstract;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Middlewares
{
    public class EmailOwnershipMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEmailLogService _emailLogService;
        private readonly AppSettings _appSettings;

        public EmailOwnershipMiddleware(RequestDelegate next, IEmailLogService emailLogService, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _emailLogService = emailLogService;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            // İstek URL'sinden emailLogId'yi alalım
            var emailLogId = context.Request.RouteValues["id"]?.ToString();

            // Eğer istek bir DELETE isteği ise ve emailLogId mevcutsa
            if (context.Request.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(emailLogId))
            {
                // EmailLog'un kullanıcıya ait olup olmadığını kontrol edelim
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (JwtHelper.TokenIsValid(token, _appSettings.SecretKey))
                {
                    (string UserEmail, string UserId) = JwtHelper.GetJwtPayloadInfo(token);
                    var emailLog = await _emailLogService.GetEmailLogByIdAsync(int.Parse(emailLogId));
                    if (emailLog == null || emailLog.UserId != int.Parse(UserId))
                    {
                        context.Response.StatusCode = 403; // Erişim reddedildi
                        await context.Response.WriteAsync("Mail kullanıcıya ait değil.");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401; // Yetkilendirme hatası
                    await context.Response.WriteAsync("Yetkilendirme hatası.");
                    return;
                }
            }

            // Sonraki middleware'e devam et
            await _next(context);
        }
    }
}
