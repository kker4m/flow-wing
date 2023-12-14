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
    public class ScheduledEmailRepository : IScheduledEmailRepository
    {
        private readonly FlowWingDbContext _dbContext;

        public ScheduledEmailRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            // Planlanan e-postayı veritabanına ekle
            _dbContext.ScheduledEmails.Add(scheduledEmail);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // Planlanan e-postayı döndür
            return scheduledEmail;
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Remove(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }

        public async Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync()
        {
            // Tüm planlanan e-postaları veritabanından getir
            var scheduledEmails = await _dbContext.ScheduledEmails.ToListAsync();

            // Planlanan e-postaları döndür
            return scheduledEmails;
        }

        public async Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id)
        {
            // Planlanan e-postayı veritabanından bul
            var scheduledEmail = await _dbContext.ScheduledEmails.FindAsync(id);

            // Planlanan e-postayı döndür
            return scheduledEmail;
        }

        public async Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            // Planlanan e-postayı veritabanında güncelle
            _dbContext.ScheduledEmails.Update(scheduledEmail);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // Planlanan e-postayı döndür
            return scheduledEmail;
        }
    }
}
