using FlowWing.Business.Abstract;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
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
            
            foreach(var recipient in recipientEmail.Split(','))
            {
                message.To.Add(MailboxAddress.Parse(recipient));
            }

            message.Subject = subject;

            var multipart = new Multipart("mixed");

            multipart.Add(new TextPart("plain") { Text = body });
            //
            //foreach (var attachmentPath in attachments)
            //{
            //   var attachment = new MimePart
            //   {
            //      Content = new MimeContent(File.OpenRead(attachmentPath), ContentEncoding.Default),
            //      ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            //      ContentTransferEncoding = ContentEncoding.Base64,
            //      FileName = Path.GetFileName(attachmentPath)
            //  };
            //
            //              multipart.Add(attachment);
            //}

            message.Body = multipart;

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_senderEmail, _senderPassword);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);

            if (_emailLog.Status == false)
            {
                _emailLog.Status = true;
                await _emailLogService.UpdateEmailLogAsync(_emailLog);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"E-posta gönderme hatası: {ex.Message}");
            // Hata yönetimi yapilabilir
        }
    }
}