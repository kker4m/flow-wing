using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Abstract
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByNameAsync(string roleName);
        Task AddAsync(Role role);
        Task UpdateAsync(Role role);
        Task DeleteAsync(int id);
    }
}
