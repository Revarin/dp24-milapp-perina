using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserLocationEntity> Locations { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
