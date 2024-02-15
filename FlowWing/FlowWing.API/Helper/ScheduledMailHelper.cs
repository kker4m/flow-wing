using System.Xml;
using FlowWing.API.Models;
using FlowWing.Business.Abstract;
using FlowWing.Entities;
using Hangfire;
using Microsoft.Extensions.Options;
using FlowWing.API.Models;
using NCrontab;

namespace FlowWing.API.Helpers;

public class ScheduledMailHelper
{
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly AppSettings _appSettings;
    private readonly EmailSenderService _emailSenderService;

    public ScheduledMailHelper(IBackgroundJobClient backgroundJobClient, IOptions<AppSettings> appSettings, EmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
        _backgroundJobClient = backgroundJobClient;
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
    }
    
    public void ScheduleRepeatingEmail(EmailLog emailLog, ScheduledRepeatingEmailModel scheduledRepeatingEmailModel)
    {
        //Hangfire'da işi planla, NextSendingDate'de ilk maili gonder
        BackgroundJob.Schedule(
            () => SendRepeatingEmail(emailLog, scheduledRepeatingEmailModel),
            scheduledRepeatingEmailModel.NextSendingDate
        );
    }
    public async Task ScheduleScheduledEmail(EmailLog createdEmailLog, ScheduledEmailLogModel scheduledEmail)
    {
        // Hangfire'da işi planla
        BackgroundJob.Schedule(() => _emailSenderService.SendEmail(scheduledEmail.RecipientsEmail,scheduledEmail.EmailSubject,scheduledEmail.EmailBody,createdEmailLog)
            ,scheduledEmail.SentDateTime);
    }
    public async Task SendRepeatingEmail(EmailLog emailLog, ScheduledRepeatingEmailModel scheduledRepeatingEmailModel)
    {
        // E-postayı gönder
        // RecipientsEmail'i virgulden ayir, her bir aliciya ayri ayri gonder
        _emailSenderService.SendEmail(emailLog.RecipientsEmail, emailLog.EmailSubject, emailLog.SentEmailBody, emailLog);

        // Eğer EndDate'den önceyse, işlemi sona erdir
        if (DateTime.UtcNow < scheduledRepeatingEmailModel.RepeatEndDate)
        {
            DateTime repeatInterval = AddUserInputToUtcNow(scheduledRepeatingEmailModel.RepeatInterval);
            // Yeniden planla
            RecurringJob.AddOrUpdate(
                $"RepeatingEmailJob_{emailLog.Id}",
                () => SendRepeatingEmail(emailLog, scheduledRepeatingEmailModel),
                Cron.MinuteInterval(repeatInterval.Minute)
            );
        }
        else
        {
            // Eğer EndDate geçmişse, planı sil
            RecurringJob.RemoveIfExists($"RepeatingEmailJob_{emailLog.Id}");
        }
    }
    
    static DateTime AddUserInputToUtcNow(string userInput)
    {
        // Kullanıcıdan alınan stringi ayırma işlemi
        string[] parts = userInput.Split('-');

        if (parts.Length != 4)
        {
            // Hatalı giriş durumu
            throw new ArgumentException("Geçersiz tarih formatı. Doğru format: Month-Day-Hour-Minute");
        }

        // Kullanıcıdan alınan bilgileri parçalara ayırıp int'e çevirme
        if (!int.TryParse(parts[0], out int month) ||
            !int.TryParse(parts[1], out int day) ||
            !int.TryParse(parts[2], out int hour) ||
            !int.TryParse(parts[3], out int minute))
        {
            throw new ArgumentException("Geçersiz tarih formatı. Doğru format: Month-Day-Hour-Minute");
        }

        // Validate that month, day, hour, and minute are within appropriate ranges
        if (month < 0 || day < 0 || hour < 0 || minute < 0)
        {
            throw new ArgumentException("Geçersiz tarih formatı. Doğru format: Month-Day-Hour-Minute");
        }

        // Kullanıcıdan alınan bilgileri DateTime nesnesine çevirme
        DateTime userDateTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, DateTimeKind.Utc);

        // Eğer değerler sıfırdan büyükse, bu değerleri DateTime.UtcNow'a ekleyip sonucu döndürme
        if (month > 0)
        {
            userDateTime = userDateTime.AddMonths(month);
        }

        if (day > 0)
        {
            userDateTime = userDateTime.AddDays(day);
        }

        if (hour > 0)
        {
            userDateTime = userDateTime.AddHours(hour);
        }

        if (minute > 0)
        {
            userDateTime = userDateTime.AddMinutes(minute);
        }

        return userDateTime;
    }

}