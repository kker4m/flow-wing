using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.DataAccess.Concrete;
using FlowWing.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Concrete
{
    public class EmailLogManager : IEmailLogService
    {
        private IEmailLogRepository _emailLogRepository;
        private IScheduledEmailRepository _scheduledEmailRepository;

        public EmailLogManager(IEmailLogRepository emailLogRepository, IScheduledEmailRepository scheduledEmailRepository)
        {
            _emailLogRepository = emailLogRepository;
            _scheduledEmailRepository = scheduledEmailRepository;
        }

        public async Task<EmailLog> CreateEmailLogAsync(EmailLog emailLog)
        {
            if (await _emailLogRepository.GetEmailLogByIdAsync(emailLog.Id) != null)
            {
                throw new Exception("EmailLog already exists");
            }
            else
            {
                await _emailLogRepository.CreateEmailLogAsync(emailLog);
                return emailLog;
            }
        }
        public EmailLog CreateEmailLog(EmailLog emailLog)
        {
            _emailLogRepository.CreateEmailLog(emailLog);
            return emailLog;
        }

        public async Task<EmailLog> DeleteEmailLogAsync(int id)
        {
            if (await _emailLogRepository.GetEmailLogByIdAsync(id) == null)
            {
                throw new Exception("EmailLog does not exist");
            }
            else
            {
                var emailLog = await _emailLogRepository.GetEmailLogByIdAsync(id);
                emailLog.DeletionDate = DateTime.UtcNow.AddDays(30);
                await _emailLogRepository.UpdateEmailLogAsync(emailLog);
                BackgroundJob.Schedule(() => _emailLogRepository.DeleteEmailLogAsync(emailLog), TimeSpan.FromDays(30));
                
                if (emailLog.repeatingLogId == null && emailLog.IsScheduled && !emailLog.Status)
                {
                    RecurringJob.RemoveIfExists($"#{emailLog.HangfireJobId}");
                    ScheduledEmail scheduledEmail = await _scheduledEmailRepository.GetScheduledEmailByEmailLogId(id);
                    if (scheduledEmail != null)
                    {
                        scheduledEmail.DeletionDate = DateTime.UtcNow.AddDays(30);
                        await _scheduledEmailRepository.UpdateScheduledEmailAsync(scheduledEmail);

                        BackgroundJob.Schedule(() => _scheduledEmailRepository.DeleteScheduledEmailAsync(scheduledEmail), TimeSpan.FromDays(30));
                    }
                }
                else if (emailLog.repeatingLogId != null)
                {
                    //later
                }


                return emailLog;
            }
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync()
        {
            return await _emailLogRepository.GetAllEmailLogsAsync();
        }
        public async Task<IEnumerable<EmailLog>> GetEmailLogsByRepeatingLogIdAsync(int repeatingLogId)
        {
            return await _emailLogRepository.GetEmailLogsByRepeatingLogIdAsync(repeatingLogId);
        }
        public async Task<EmailLog> GetEmailLogByIdAsync(int? id)
        { 
          return await _emailLogRepository.GetEmailLogByIdAsync(id);
        }

        public async Task<EmailLog> GetEmailLogByScheduledEmailIdAsync(int scheduledEmailId)
        { 
            return await _emailLogRepository.GetEmailLogByScheduledEmailIdAsync(scheduledEmailId);
        }
        public async Task<IEnumerable<EmailLog>> GetEmailLogsByUserIdAsync(int userId)
        {
            return await _emailLogRepository.GetEmailLogsByUserIdAsync(userId);
        }
        
        public async Task<IEnumerable<EmailLog>> GetEmailLogsByRecipientsEmailAsync(string recipientEmail)
        {   
            return await _emailLogRepository.GetEmailLogsByRecipientsEmailAsync(recipientEmail);
        }
        public async Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog)
        {
            
            if (await _emailLogRepository.GetEmailLogByIdAsync(emailLog.Id) == null)
            {
                throw new Exception("EmailLog does not exist");
            }
            else
            {
                await _emailLogRepository.UpdateEmailLogAsync(emailLog);
                return emailLog;
            }
        }

        public EmailLog UpdateEmailLog(EmailLog emailLog)
        {
            if (_emailLogRepository.GetEmailLogById(emailLog.Id) == null)
            {
                throw new Exception("EmailLog does not exist");
            }
            else
            {
                _emailLogRepository.UpdateEmailLog(emailLog);
                return emailLog;
            }
        }
    }
}
