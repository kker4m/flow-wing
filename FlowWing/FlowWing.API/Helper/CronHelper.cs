using System;

namespace FlowWing.API.Helpers
{
    public static class CronHelper
    {
        public static string GenerateCronExpression(
            DateTime nextSendingDate,
            TimeSpan repeatInterval,
            DateTime repeatEndDate)
        {
            // Başlangıç tarihinden itibaren geçen süre
            TimeSpan elapsed = nextSendingDate.ToUniversalTime() - DateTime.UtcNow;

            // Crontab ifadesini oluştur
            string cronExpression = $"{(int)elapsed.TotalMinutes} */{repeatInterval.TotalMinutes} * * *";

            // Eğer bitiş tarihi belirlenmişse, sonlandırma tarihini de ekle
            if (repeatEndDate != DateTime.MaxValue)
            {
                TimeSpan duration = repeatEndDate.ToUniversalTime() - nextSendingDate.ToUniversalTime();
                cronExpression += $" && {(int)duration.TotalMinutes} * * *";
            }

            return cronExpression;
        }
    }
}