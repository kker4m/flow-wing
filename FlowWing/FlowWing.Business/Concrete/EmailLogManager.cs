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
            return await _emailLogRepository.CreateEmailLogAsync(emailLog);
        }

        public async Task<EmailLog> DeleteEmailLogAsync(int id)
        {
            EmailLog log = await _emailLogRepository.GetEmailLogByIdAsync(id);
            if (log == null)
            {
                throw new Exception("Email bulunamadi");
            }
            else
            {
                await _emailLogRepository.DeleteEmailLogAsync(log);
                return log;
            }
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync()
        {
            return await _emailLogRepository.GetAllEmailLogsAsync();

        }

        public async Task<EmailLog> GetEmailLogByIdAsync(int id)
        {
            EmailLog log = await _emailLogRepository.GetEmailLogByIdAsync(id);
            if (log  != null)
            {
                return log;
            }
            throw new Exception("Log bulunamadi");
        }

        public async Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog)
        {
            await _emailLogRepository.UpdateEmailLogAsync(emailLog);
            return emailLog;
        }
    }
}
