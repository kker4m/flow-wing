using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Abstract
{
    public interface IRepeatingMailRepository
    {
        Task<RepeatingMail> CreateRepeatingMailAsync(RepeatingMail repeatingMail);
        Task<RepeatingMail> UpdateRepeatingMailAsync(RepeatingMail repeatingMail);
        Task<RepeatingMail> DeleteRepeatingMailAsync(RepeatingMail repeatingMail);

        Task<RepeatingMail> GetRepeatingMailByIdAsync(int id);
        Task<IEnumerable<RepeatingMail>> GetAllRepeatingMails();
    }
}
