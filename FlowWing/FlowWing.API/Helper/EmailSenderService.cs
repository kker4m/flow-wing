using FlowWing.API.Controllers;
using FlowWing.Business.Abstract;
using FlowWing.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Protocol;

namespace FlowWing.API.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowWing.Entities;

public class EmailSenderService
{
    private IEmailLogService _emailLogService;
    private readonly AppSettings _appSettings;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _senderEmail;
    private readonly string _senderPassword;

    public EmailSenderService(IOptions<AppSettings> appSettings, IEmailLogService emailLogService)
    {
        _emailLogService = emailLogService;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));

        _smtpServer = _appSettings.EmailConnectionServices._smtpServer;
        _smtpPort = _appSettings.EmailConnectionServices._smtpPort;
        _senderEmail = _appSettings.EmailConnectionServices._senderEmail;
        _senderPassword = _appSettings.EmailConnectionServices._senderPassword;
    }

    public async Task SendEmail(string recipientEmail, string subject, string body, EmailLog _emailLog)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_senderEmail));
            message.To.Add(MailboxAddress.Parse(recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_senderEmail, _senderPassword);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
            _emailLog.Status = true;
            await _emailLogService.UpdateEmailLogAsync(_emailLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
            // Hata yönetimi yapilabilir
        }
    }
}