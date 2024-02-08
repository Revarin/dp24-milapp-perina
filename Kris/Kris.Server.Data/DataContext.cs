using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>().HasKey(e => e.Id);

        modelBuilder.Entity<SessionEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<SessionEntity>()
            .HasMany(e => e.Users)
            .WithOne(e => e.Session)
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionUserEntity>().HasKey(e => new { e.UserId, e.SessionId });
        modelBuilder.Entity<SessionUserEntity>()
            .HasOne(e => e.Session)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.SessionId);
        modelBuilder.Entity<SessionUserEntity>()
            .HasOne(e => e.User)
            .WithOne();

        base.OnModelCreating(modelBuilder);
    }
}
