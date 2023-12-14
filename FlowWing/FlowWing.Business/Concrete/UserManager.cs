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
            await _userRepository.CreateUserAsync(user);
            return user;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new Exception("silinmek istenen user bulunamadi");
            }
            else
            {
                await _userRepository.DeleteUserAsync(user);
                return user;
            }
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (await _userRepository.GetUserByIdAsync(user.Id) != null)
            {
                return await _userRepository.UpdateUserAsync(user);
            }
            throw new Exception("user bulunamadi");
        }
    }
}
