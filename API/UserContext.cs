using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class UserContext(DbContextOptions<UserContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public virtual DbSet<User> Users { get; set; }
    }
}
