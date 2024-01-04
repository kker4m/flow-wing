using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
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

        public EmailLogManager(IEmailLogRepository emailLogRepository)
        {
            _emailLogRepository = emailLogRepository;
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

        public async Task<EmailLog> DeleteEmailLogAsync(int id)
        {
            if (await _emailLogRepository.GetEmailLogByIdAsync(id) == null)
            {
                throw new Exception("EmailLog does not exist");
            }
            else
            {
                var emailLog = await _emailLogRepository.GetEmailLogByIdAsync(id);
                await _emailLogRepository.DeleteEmailLogAsync(emailLog);
                return emailLog;
            }
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync()
        {
            return await _emailLogRepository.GetAllEmailLogsAsync();
        }

        public async Task<EmailLog> GetEmailLogByIdAsync(int id)
        { 
          return await _emailLogRepository.GetEmailLogByIdAsync(id);
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
    }
}
