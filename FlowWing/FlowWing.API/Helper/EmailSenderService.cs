using FlowWing.Business.Abstract;
using Microsoft.Extensions.Options;

namespace FlowWing.API.Helpers;
using System;
using System.Threading.Tasks;
using FlowWing.Entities;
using FlowWing.API.Controllers;
using FlowWing.DataAccess.Abstract;
using FlowWing.API.Models;
using NuGet.Common;
using System.Net.Mail;

public class EmailSenderService
{
    private IEmailLogService _emailLogService;
    private IUserService _userRepository;

    public EmailSenderService(IEmailLogService emailLogService, IUserService userRepository)
    {
        _emailLogService = emailLogService;
        _userRepository = userRepository;
    }

    public async Task UpdateStatus(EmailLog _emailLog)
    {
        if (_emailLog.Status == false)
        {
            _emailLog.Status = true;
            await _emailLogService.UpdateEmailLogAsync(_emailLog);
        }
    }

    public async Task CreateRepeatingEmailLog(EmailLog emailLog,int scheduledEmailId)
    {
        EmailLog createdEmailLog;
        string? attachmentIds = "";
        User user = await _userRepository.GetUserByIdAsync(emailLog.UserId);
        createdEmailLog = new EmailLog
        {
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            SentDateTime = DateTime.UtcNow,
            RecipientsEmail = emailLog.RecipientsEmail,
            SenderEmail = emailLog.SenderEmail,
            EmailSubject = emailLog.EmailSubject,
            SentEmailBody = emailLog.SentEmailBody,
            Status = true,
            IsScheduled = true,
            User = user,
            repeatingLogId = scheduledEmailId
        };

        createdEmailLog = _emailLogService.CreateEmailLog(createdEmailLog);
        createdEmailLog.HangfireJobId = $"repeatingemailjob-{createdEmailLog.Id}";

        attachmentIds = emailLog.AttachmentIds;

        if (attachmentIds != null)
        {
            if (attachmentIds.Length > 0)
            {
                attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                createdEmailLog.AttachmentIds = attachmentIds;
            }
        }

        await _emailLogService.UpdateEmailLogAsync(createdEmailLog);
    }
}