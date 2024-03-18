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
    private readonly EmailSenderService _emailSenderService;
    private readonly IScheduledEmailService _scheduledEmailService;

    public ScheduledMailHelper(EmailSenderService emailSenderService, IScheduledEmailService scheduledEmailService)
    {
        _emailSenderService = emailSenderService;
        _scheduledEmailService = scheduledEmailService;
    }
    
    public void ScheduleRepeatingEmail(EmailLog emailLog, ScheduledRepeatingEmailModel scheduledRepeatingEmailModel)
    {
        _emailSenderService.UpdateStatus(emailLog);
        //Hangfire'da işi planla, NextSendingDate'de ilk maili gonder
        BackgroundJob.Schedule($"ScheduledEmailJob_{emailLog.Id}",
            () => SendRepeatingEmail(emailLog),
            scheduledRepeatingEmailModel.NextSendingDate
        );
    }
    public async Task ScheduleScheduledEmail(EmailLog createdEmailLog, ScheduledEmailLogModel scheduledEmail)
    {
        // Hangfire'da işi planla
        BackgroundJob.Schedule($"ScheduledEmailJob_{createdEmailLog.Id}",() => _emailSenderService.UpdateStatus(createdEmailLog)
            ,scheduledEmail.SentDateTime);
    }
    public async Task SendRepeatingEmail(EmailLog emailLog)
    {
        ScheduledEmail scheduledEmail = await _scheduledEmailService.GetScheduledEmailByEmailLogId(emailLog.Id);
        await _emailSenderService.CreateRepeatingEmailLog(emailLog,scheduledEmail.Id);

        if (DateTime.UtcNow < scheduledEmail.RepeatEndDate)
        {
            _emailSenderService.UpdateStatus(emailLog);
            scheduledEmail.LastSendingDate = DateTime.UtcNow;
            await _scheduledEmailService.UpdateScheduledEmailAsync(scheduledEmail);
            scheduledEmail.NextSendingDate = AddUserInputToUtcNow(scheduledEmail.RepeatInterval);
            int minutes = ConvertUserInputToMinutes(scheduledEmail.RepeatInterval);

            RecurringJob.AddOrUpdate(
                $"ScheduledEmailJob_{emailLog.Id}",
                () => SendRepeatingEmail(emailLog),
                Cron.MinuteInterval(minutes)
            );
        }
        else
        {
            RecurringJob.RemoveIfExists($"ScheduledEmailJob_{emailLog.Id}");
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
    static int ConvertUserInputToMinutes(string userInput)
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

        // Kullanıcıdan alınan bilgileri dakika cinsinden toplama
        int totalMinutes = 0;
        totalMinutes += month * 30 * 24 * 60;  // Her ayı 30 gün olarak kabul ediyoruz
        totalMinutes += day * 24 * 60;
        totalMinutes += hour * 60;
        totalMinutes += minute;

        return totalMinutes;
    }
}