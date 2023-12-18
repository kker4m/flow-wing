using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Abstract
{
    public interface IEmailLogRepository
    {
        Task<EmailLog> CreateEmailLogAsync(EmailLog emailLog);
        Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog);
        Task<EmailLog> DeleteEmailLogAsync(EmailLog emailLog);

        Task<EmailLog> GetEmailLogByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailLogAsync(EmailLog emailLog);

        Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync();
        Task<IEnumerable<EmailLog>> GetAllEmailLogsByUserIdAsync(int id);
        Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsByUserIdAsync(int id);
    }
}
