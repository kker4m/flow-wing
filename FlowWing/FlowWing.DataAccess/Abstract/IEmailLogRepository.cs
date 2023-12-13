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
        Task<EmailLog> GetEmailLogByIdAsync(int id);
        Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync();
        Task<EmailLog> CreateEmailLogAsync(EmailLog emailLog);
        Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog);
        Task<EmailLog> DeleteEmailLogAsync(int id);
    }
}
