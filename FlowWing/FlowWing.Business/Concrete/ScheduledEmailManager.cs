using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Concrete
{
    public class ScheduledEmailManager : IScheduledEmailService
    {
        private readonly IScheduledEmailRepository _scheduledEmailRepository;
        private readonly IEmailLogRepository _emailLogRepository;
        public ScheduledEmailManager(IScheduledEmailRepository scheduledEmailRepository, IEmailLogRepository emailLogRepository) 
        { 
            _scheduledEmailRepository = scheduledEmailRepository;
            _emailLogRepository = emailLogRepository;
        }

        public async Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            return await _scheduledEmailRepository.CreateScheduledEmailAsync(scheduledEmail);
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailAsync(int id)
        {
            ScheduledEmail scheduledEmail = await _scheduledEmailRepository.GetScheduledEmailByIdAsync(id);
            EmailLog emailLog = await _emailLogRepository.GetEmailLogByIdAsync(scheduledEmail.EmailLogId);
            if (scheduledEmail != null)
            {
                emailLog.DeletionDate = DateTime.Now.AddDays(30);
                RecurringJob.RemoveIfExists("ScheduledEmailJob_" + id.ToString());
                RecurringJob.RemoveIfExists("ScheduledEmailJob_" + emailLog.Id.ToString());

                //schedule the deletion of emailLog 30 days later
                BackgroundJob.Schedule(() => _emailLogRepository.DeleteEmailLogAsync(emailLog), TimeSpan.FromDays(30));
                BackgroundJob.Schedule(() => _scheduledEmailRepository.DeleteScheduledEmailAsync(scheduledEmail), TimeSpan.FromDays(30));
                return scheduledEmail;
            }
            else
            {
                   return null;
            }
        } 

        public async Task<ScheduledEmail> DeleteScheduledRepeatingEmailAsync(int id)
        {
            ScheduledEmail scheduledEmail = await _scheduledEmailRepository.GetScheduledEmailByIdAsync(id);
            if (scheduledEmail != null)
            {
                //find all emailLogs which is have repeatingLogId equals to scheduledEmail.Id, make theri DeletionDate column to datetime.now
                var emailLogs = await _emailLogRepository.GetEmailLogsByRepeatingLogIdAsync(scheduledEmail.Id);
                foreach (var emailLog in emailLogs)
                {
                    emailLog.DeletionDate = DateTime.Now.AddDays(30);
                    await _emailLogRepository.UpdateEmailLogAsync(emailLog);
                    BackgroundJob.Schedule(() => _emailLogRepository.DeleteEmailLogAsync(emailLog), TimeSpan.FromDays(30));
                    RecurringJob.RemoveIfExists("ScheduledEmailJob_" + emailLog.Id.ToString());
                }
                BackgroundJob.Schedule(() => _scheduledEmailRepository.DeleteScheduledEmailAsync(scheduledEmail), TimeSpan.FromDays(30));
                RecurringJob.RemoveIfExists("ScheduledEmailJob_" + id.ToString());
                return scheduledEmail;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<ScheduledEmail>> GetRepeatingScheduledMailsAsync()
        {
            return await _scheduledEmailRepository.GetRepeatingScheduledMailsAsync();
        }

        public async Task<ScheduledEmail> GetScheduledEmailByEmailLogId(int id)
        {
            return await _scheduledEmailRepository.GetScheduledEmailByEmailLogId(id);
        }

        public async Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync()
        {
            return await _scheduledEmailRepository.GetAllScheduledEmailsAsync();
        }

        public async Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id)
        {
            return await _scheduledEmailRepository.GetScheduledEmailByIdAsync(id);
        }

        public async Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            return await _scheduledEmailRepository.UpdateScheduledEmailAsync(scheduledEmail);
        }
    }
}
