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
    public class RoleManager : IRoleService
    {
        IRoleRepository _roleRepository;
        public RoleManager(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task AddAsync(Role role)
        {
            await _roleRepository.AddAsync(role);
        }

        public async Task DeleteAsync(int id)
        {
            await _roleRepository.DeleteAsync(id);
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _roleRepository.GetByNameAsync(roleName);
        }

        public async Task UpdateAsync(Role role)
        {
            await _roleRepository.UpdateAsync(role);
        }
    }
}
