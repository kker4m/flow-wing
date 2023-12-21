using FlowWing.Business.Abstract;
using FlowWing.DataAccess.Abstract;
using FlowWing.Entities;

namespace FlowWing.Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (await _userRepository.GetUserByEmailAsync(user.Email) == null | await _userRepository.GetUserByUsernameAsync(user.Username) == null)
            {
                throw new Exception("User already exists");
            }
            else
            {
                _userRepository.CreateUserAsync(user);
                return user;
            }
        }

        public async Task<User> DeleteUserAsync(User user)
        {
            if (await _userRepository.GetUserByIdAsync(user.Id) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                await _userRepository.DeleteUserAsync(user);
                return user;
            }
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsByIdAsync(int id)
        {
            if(await _userRepository.GetUserByIdAsync(id) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                return await _userRepository.GetAllEmailLogsByIdAsync(id);
            }
        }

        public async Task<IEnumerable<EmailLog>> GetAllEmailLogsByUserAsync(User user)
        {
            if (await _userRepository.GetUserByIdAsync(user.Id) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                return await _userRepository.GetAllEmailLogsByIdAsync(user.Id);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if ( await _userRepository.GetUserByEmailAsync(email) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                return await _userRepository.GetUserByEmailAsync(email);
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if ( await _userRepository.GetUserByIdAsync(id) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                return await _userRepository.GetUserByIdAsync(id);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if ( await _userRepository.GetUserByUsernameAsync(username) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                return await _userRepository.GetUserByUsernameAsync(username);
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (await _userRepository.GetUserByIdAsync(user.Id) == null)
            {
                throw new Exception("User does not exist");
            }
            else
            {
                await _userRepository.UpdateUserAsync(user);
                return user;
            }
        }
    }
}
