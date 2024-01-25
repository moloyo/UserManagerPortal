using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer
{
    public class UserContext : DbContext
    {
        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
