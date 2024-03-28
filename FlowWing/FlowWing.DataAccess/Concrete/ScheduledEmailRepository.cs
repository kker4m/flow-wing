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

        public async Task<IEnumerable<ScheduledEmail>> GetRepeatingScheduledMailsAsync()
        {
            return await _dbContext.ScheduledEmails.AsNoTracking().Where(x => x.IsRepeating).ToListAsync();
        }

        public async Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Add(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Remove(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailByEmailLogIdAsync(int id)
        {
            var scheduledEmail = await _dbContext.ScheduledEmails.FirstOrDefaultAsync(x => x.EmailLogId == id);
            _dbContext.ScheduledEmails.Remove(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }


        public async Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync()
        {
            return await _dbContext.ScheduledEmails.AsNoTracking().ToListAsync();
        }
        

        public async Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id)
        {
            return await _dbContext.ScheduledEmails.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<ScheduledEmail> GetScheduledEmailByEmailLogId(int id)
        {
            return await _dbContext.ScheduledEmails.AsNoTracking().FirstOrDefaultAsync(x => x.EmailLogId == id);
        }

        public async Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Update(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }
    }
}
