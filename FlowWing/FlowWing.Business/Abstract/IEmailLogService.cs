﻿using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Abstract
{
    public interface IEmailLogService
    {
        Task<EmailLog> GetEmailLogByIdAsync(int? id);
        Task<IEnumerable<EmailLog>> GetAllEmailLogsAsync();
        Task<IEnumerable<EmailLog>> GetEmailLogsByRepeatingLogIdAsync(int repeatingLogId);
        Task<EmailLog> CreateEmailLogAsync(EmailLog emailLog);
        EmailLog CreateEmailLog(EmailLog emailLog);
        Task<EmailLog> UpdateEmailLogAsync(EmailLog emailLog);
        EmailLog UpdateEmailLog(EmailLog emailLog);
        Task<EmailLog> DeleteEmailLogAsync(int id);
        Task <EmailLog> GetEmailLogByScheduledEmailIdAsync(int scheduledEmailId);
        Task<IEnumerable<EmailLog>> GetEmailLogsByUserIdAsync(int userId);
        Task<IEnumerable<EmailLog>> GetEmailLogsByRecipientsEmailAsync(string recipientEmail);
    }
}
