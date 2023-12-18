﻿using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Concrete
{
    public class ScheduledEmailRepository : IScheduledEmailRepository
    {
        private readonly FlowWingDbContext _dbContext;

        public ScheduledEmailRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ScheduledEmail> CreateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Add(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }

        public async Task<ScheduledEmail> DeleteScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Remove(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }

        public async Task<IEnumerable<ScheduledEmail>> GetAllScheduledEmailsAsync()
        {
            return await _dbContext.ScheduledEmails.ToListAsync();
        }

        public async Task<RepeatingMail> GetRepeatingMailByIdAsync(int id)
        {
            var scheduledEmail = await _dbContext.ScheduledEmails.FindAsync(id);
            return await _dbContext.RepeatingMails.FindAsync(scheduledEmail.RepeatingMail.Id);
        }

        public async Task<ScheduledEmail> GetScheduledEmailByIdAsync(int id)
        {
            return await _dbContext.ScheduledEmails.FindAsync(id);
        }

        public async Task<ScheduledEmail> UpdateScheduledEmailAsync(ScheduledEmail scheduledEmail)
        {
            _dbContext.ScheduledEmails.Update(scheduledEmail);
            await _dbContext.SaveChangesAsync();
            return scheduledEmail;
        }
    }
}
