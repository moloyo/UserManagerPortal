using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class UserContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }

        public UserContext()
        {
            
        }

        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
