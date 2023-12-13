using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Concrete
{
    public class EmailLogRepository : IEmailLogRepository
    {

        private readonly FlowWingDbContext _dbContext;

        public EmailLogRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmailLog> CreateEmailLogAsync(EmailLog emailLog)
        {
            // E-posta günlüğünü veritabanına ekle
            _dbContext.EmailLogs.Add(emailLog);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // E-posta günlüğünü döndür
            return emailLog;
        }

        public async Task<EmailLog> DeleteEmailLogAsync(int id)
        {
            // E-posta günlüğünü veritabanından bul
            var emailLog = await _dbContext.EmailLogs.FindAsync(id);

            // E-posta günlüğünü sil
            if (emailLog != null)
            {
                _dbContext.EmailLogs.Remove(emailLog);
                await _dbContext.SaveChangesAsync();
            }

            // E-posta günlüğünü döndür
            return emailLog;
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync()
        {
            // Tüm e-posta günlüklerini veritabanından getir
            var emailLogs = await _dbContext.EmailLogs.ToListAsync();

            // E-posta günlüklerini döndür
            return emailLogs;
        }

        public async Task<EmailLog> GetEmailLogByIdAsync(int id)
        {
            // E-posta günlüğünü veritabanından bul
            var emailLog = await _dbContext.EmailLogs.FindAsync(id);

            // E-posta günlüğünü döndür
            return emailLog;
        }

        public async Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog)
        {
            // E-posta günlüğünü veritabanında güncelle
            _dbContext.EmailLogs.Update(emailLog);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // E-posta günlüğünü döndür
            return emailLog;
        }
    }
}
