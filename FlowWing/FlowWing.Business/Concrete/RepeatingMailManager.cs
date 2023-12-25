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
    public class RepeatingMailManager : IRepeatingMailService
    {
        private readonly IRepeatingMailRepository _repeatingMailRepository;
        public RepeatingMailManager(IRepeatingMailRepository repeatingMailRepository)
        {
            _repeatingMailRepository = repeatingMailRepository;
        }
        public async Task<RepeatingMail> CreateRepeatingMailAsync(RepeatingMail repeatingMail)
        {
            return await _repeatingMailRepository.CreateRepeatingMailAsync(repeatingMail);
        }

        public async Task<RepeatingMail> DeleteRepeatingMailAsync(int id)
        {
            RepeatingMail repeatingMail = await _repeatingMailRepository.GetRepeatingMailByIdAsync(id);
            return await _repeatingMailRepository.DeleteRepeatingMailAsync(repeatingMail);


        }

        public async Task<IEnumerable<RepeatingMail>> GetAllRepeatingMailsAsync()
        {
            return await _repeatingMailRepository.GetAllRepeatingMailsAsync();
        }

        public async Task<RepeatingMail> GetRepeatingMailByIdAsync(int id)
        {
            if (id > 0)
            {
                return await _repeatingMailRepository.GetRepeatingMailByIdAsync(id);
            }
            else
            {
                throw new Exception("Id must be greater than 0");
            }
        }

        public async Task<RepeatingMail> GetRepeatingMailBySenderEmailAsync(string senderEmail)
        {
            if (senderEmail != null)
            {
                return await _repeatingMailRepository.GetRepeatingMailBySenderEmailAsync(senderEmail);
            }
            else
            {
                throw new Exception("Sender Email must not be null");
            }
        }

        public async Task<RepeatingMail> UpdateRepeatingMailAsync(RepeatingMail repeatingMail)
        {
            if (await _repeatingMailRepository.GetRepeatingMailByIdAsync(repeatingMail.Id) == null)
            {
                throw new Exception("Repeating Mail not found");
            }
            return await _repeatingMailRepository.UpdateRepeatingMailAsync(repeatingMail);
 
        }
    }
}
