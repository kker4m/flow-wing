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
            return await _dbContext.EmailLogs.ToListAsync();
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsByUserIdAsync(int id)
        {
            return await _dbContext.EmailLogs.Where(x => x.User.Id == id).ToListAsync();
        }

        //Test et
        public async Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsByUserIdAsync(int id)
        {
            IEnumerable<EmailLog> logs = await _dbContext.EmailLogs
                .Include(x => x.ScheduledEmail) // ScheduledEmail bağlantısını dahil et
                .Where(x => x.User.Id == id)
                .ToListAsync();

            IEnumerable<ScheduledEmail> scheduledEmails = logs.Select(log => log.ScheduledEmail);
            return scheduledEmails;
        }

        public async Task<EmailLog> GetEmailLogByIdAsync(int id)
        {
            return await _dbContext.EmailLogs.FindAsync(id);
        }

        public async Task<User> GetUserByEmailLogAsync(EmailLog emailLog)
        {
            return await _dbContext.Users.FindAsync(emailLog.User.Id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            int user_id = _dbContext.EmailLogs.FindAsync(id).Result.User.Id;
            return await _dbContext.Users.FindAsync(user_id);
        }

        public async Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog)
        {
            _dbContext.EmailLogs.Update(emailLog);
            await _dbContext.SaveChangesAsync();
            return emailLog;
        }
    }
}
