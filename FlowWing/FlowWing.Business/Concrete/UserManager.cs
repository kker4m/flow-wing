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
            if (await _userRepository.GetUserByEmailAsync(user.Email) != null | await _userRepository.GetUserByUsernameAsync(user.Username) != null)
            {
                throw new Exception("User already exists");
            }
            else
            {
                string hashedPassword = PasswordHasher.HashPassword(user.Password);
                user.Password = hashedPassword;
                user.CreationDate = user.CreationDate.ToUniversalTime();
                user.LastLoginDate = user.LastLoginDate.ToUniversalTime();
                await _userRepository.CreateUserAsync(user);
                return user;
            }
        }   
        public User CreateUser(User user)
        {
            _userRepository.CreateUser(user);
            return user;
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
        

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            User user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User _searchingUser = await _userRepository.GetUserByIdAsync(user.Id);
            await _userRepository.UpdateUserAsync(_searchingUser);
            return _searchingUser;
        }
    }
}
