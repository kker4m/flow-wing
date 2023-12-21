using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Abstract
{
    public interface IRepeatingMailService
    {
        Task<RepeatingMail> CreateRepeatingMailAsync(RepeatingMail repeatingMail);
        Task<RepeatingMail> UpdateRepeatingMailAsync(RepeatingMail repeatingMail);
        Task<RepeatingMail> DeleteRepeatingMailAsync(int id);

        Task<RepeatingMail> GetRepeatingMailByIdAsync(int id);
        Task<RepeatingMail> GetRepeatingMailBySenderEmailAsync(string senderEmail);

        Task<IEnumerable<RepeatingMail>> GetAllRepeatingMailsAsync();
    }
}
