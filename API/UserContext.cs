using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
