using API.Models;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<User?> GetAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> UserExistsAsync(Guid id);
    }
}
