using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Abstract
{
    public interface IScheduledEmailRepository
    {
        Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail);
        Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail);
        Task<ScheduledEmail> DeleteScheduledEmailAsync(ScheduledEmail scheduledEmail);

        Task<IEnumerable<ScheduledEmail>> GetActiveScheduledMailsAsync();
        Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id);
        Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync();
        Task<RepeatingMail> GetRepeatingMailByIdAsync(int id);
    }
}
