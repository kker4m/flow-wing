using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Abstract
{
    public interface IScheduledEmailService
    {
        Task<IEnumerable<ScheduledEmail>> GetRepeatingScheduledMailsAsync();
        Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id);
        Task<ScheduledEmail> GetScheduledEmailByEmailLogId(int id);
        Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync();
        Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail);
        Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail);
        Task<ScheduledEmail> DeleteScheduledEmailAsync(int id);
        Task<ScheduledEmail> DeleteScheduledEmailByEmailLogIdAsync(int id);
        Task<ScheduledEmail> DeleteScheduledRepeatingEmailAsync(int id);
    }
}
