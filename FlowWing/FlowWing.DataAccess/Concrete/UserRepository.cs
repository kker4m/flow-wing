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
    public class UserRepository : IUserRepository
    {
        private readonly FlowWingDbContext _dbContext;

        public UserRepository(FlowWingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Kullanıcıyı veritabanına ekle
            _dbContext.Users.Add(user);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // Kullanıcıyı döndür
            return user;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            // Kullanıcıyı veritabanından bul
            var user = await _dbContext.Users.FindAsync(id);

            // Kullanıcıyı sil
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }

            // Kullanıcıyı döndür
            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Tüm kullanıcıları veritabanından getir
            var users = await _dbContext.Users.ToListAsync();

            // Kullanıcıları döndür
            return users;
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            // Kullanıcıyı veritabanından bul
            var user = await _dbContext.Users.FindAsync(id);

            // Kullanıcıyı döndür
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            // Kullanıcıyı veritabanında güncelle
            _dbContext.Users.Update(user);

            // Veritabanı değişikliklerini kaydet
            await _dbContext.SaveChangesAsync();

            // Kullanıcıyı döndür
            return user;
        }
    }
}
