using FlowWing.DataAccess;
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
    private readonly AppSettings _appSettings;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _senderEmail;
    private readonly string _senderPassword;

    public EmailSenderService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));

        _smtpServer = _appSettings.EmailConnectionServices._smtpServer;
        _smtpPort = _appSettings.EmailConnectionServices._smtpPort;
        _senderEmail = _appSettings.EmailConnectionServices._senderEmail;
        _senderPassword = _appSettings.EmailConnectionServices._senderPassword;
    }

    public void SendEmail(string recipientEmail, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_senderEmail));
            message.To.Add(MailboxAddress.Parse(recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_senderEmail, _senderPassword);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
            // Hata yönetimi yapabilirsiniz
        }
    }
}