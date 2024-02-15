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
            await _dbContext.EmailLogs.AddAsync(emailLog);
            await _dbContext.SaveChangesAsync();
            return emailLog;
        }

        public async Task<EmailLog> DeleteEmailLogAsync(EmailLog emailLog)
        {
            _dbContext.EmailLogs.Remove(emailLog);
            await _dbContext.SaveChangesAsync();
            return emailLog;
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync()
        {
            return await _dbContext.EmailLogs.AsNoTracking().ToListAsync();
        }
        
        public async Task<EmailLog> GetEmailLogByIdAsync(int id)
        {
            return await _dbContext.EmailLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public EmailLog GetEmailLogById(int id)
        {
            return _dbContext.EmailLogs.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }
        public async Task<IEnumerable<EmailLog>> GetEmailLogsByRecipientsEmailAsync(string recipientEmail)
        {
            return await _dbContext.EmailLogs.AsNoTracking().Where(x => x.RecipientsEmail.Contains(recipientEmail)).ToListAsync();
        }
        
        public async Task<IEnumerable<EmailLog>> GetEmailLogsByUserIdAsync(int userId)
        {
            //Return all the email logs that user have as a list
            return await _dbContext.EmailLogs.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }
        
        public async Task<EmailLog> GetEmailLogByScheduledEmailIdAsync(int scheduledEmailId)
        {
            return await _dbContext.ScheduledEmails.AsNoTracking().Where(x => x.Id == scheduledEmailId).Select(x => x.EmailLog).FirstOrDefaultAsync();
        }
        
        public async Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog)
        {
            _dbContext.Entry(emailLog).State = EntityState.Modified;
            _dbContext.EmailLogs.Update(emailLog);
            await _dbContext.SaveChangesAsync();
            return emailLog;
        }

        public EmailLog UpdateEmailLog(EmailLog emailLog)
        {
            _dbContext.Entry(emailLog).State = EntityState.Modified;
            _dbContext.EmailLogs.Update(emailLog);
            _dbContext.Entry(emailLog).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return emailLog;
        }
    }
}
