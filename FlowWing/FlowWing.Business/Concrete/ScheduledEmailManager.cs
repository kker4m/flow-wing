using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
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
        public ScheduledEmailManager(IScheduledEmailRepository scheduledEmailRepository) 
        { 
            _scheduledEmailRepository = scheduledEmailRepository;
        }

        public async Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            return await _scheduledEmailRepository.CreateScheduledEmailAsync(scheduledEmail);
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailAsync(int id)
        {
            ScheduledEmail scheduledEmail = await _scheduledEmailRepository.GetScheduledEmailByIdAsync(id);
            if (scheduledEmail != null)
            {
                return await _scheduledEmailRepository.DeleteScheduledEmailAsync(scheduledEmail);
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
