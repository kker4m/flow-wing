using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.DataAccess.Concrete
{
    public class RoleRepository : IRoleRepository
    {
        private readonly FlowWingDbContext _dbContext;
        public RoleRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Role role)
        {
            _dbContext.Role.Add(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);
            _dbContext.Role.Remove(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _dbContext.Role.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _dbContext.Role.FindAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbContext.Role.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task UpdateAsync(Role role)
        {
            _dbContext.Role.Update(role);
            await _dbContext.SaveChangesAsync();
        }
    }
}
