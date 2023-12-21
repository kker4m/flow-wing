using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business.Abstract
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> DeleteUserAsync(User user);

        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);

        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<IEnumerable<EmailLog>> GetAllEmailLogsByUserAsync(User user);
        Task<IEnumerable<EmailLog>> GetAllEmailLogsByIdAsync(int id);
    }
}
