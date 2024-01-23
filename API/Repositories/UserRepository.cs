using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository(UserContext context) : IUserRepository
    {
        public async Task<User> CreateAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<User?> GetAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            context.Update(user);

            await context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(Guid id)
        {
            return await context.Users.AnyAsync(u => u.Id == id);
        }
    }
}
