using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;

namespace FlowWing.DataAccess.Concrete
{
    public class RepeatingMailRepository:IRepeatingMailRepository
    {
        private readonly FlowWingDbContext _dbContext;

        public RepeatingMailRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RepeatingMail> CreateRepeatingMailAsync(RepeatingMail repeatingMail)
        {
            _dbContext.RepeatingMails.Add(repeatingMail);
            await _dbContext.SaveChangesAsync();
            return repeatingMail;
        }

        public async Task<RepeatingMail> DeleteRepeatingMailAsync(RepeatingMail repeatingMail)
        {
            _dbContext.RepeatingMails.Remove(repeatingMail);
            await _dbContext.SaveChangesAsync();
            return repeatingMail;
        }

        public async Task<IEnumerable<RepeatingMail>> GetAllRepeatingMailsAsync()
        {
            return _dbContext.RepeatingMails.ToList();
        }

        public async Task<RepeatingMail> GetRepeatingMailByIdAsync(int id)
        {
            return _dbContext.RepeatingMails.FirstOrDefault(x => x.Id == id);
        }

        public async Task<RepeatingMail> GetRepeatingMailBySenderEmailAsync(string senderEmail)
        {
            return _dbContext.RepeatingMails.FirstOrDefault(x => x.SenderEmail == senderEmail);
        }
        public async Task<RepeatingMail> UpdateRepeatingMailAsync(RepeatingMail repeatingMail)
        {
            _dbContext.RepeatingMails.Update(repeatingMail);
            await _dbContext.SaveChangesAsync();
            return repeatingMail;
        }
    }
}
