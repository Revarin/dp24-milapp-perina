using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }
    public DbSet<SessionUserEntity> SessionUsers { get; set; }
    public DbSet<UserPositionEntity> UserPositions { get; set; }

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
        modelBuilder.Entity<SessionUserEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<UserPositionEntity>().HasKey(e => e.Id);

        modelBuilder.Entity<SessionUserEntity>()
            .HasOne(e => e.User)
            .WithMany(e => e.AllSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SessionUserEntity>()
            .HasOne(e => e.Session)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasOne(e => e.CurrentSession)
            .WithMany()
            .HasForeignKey(e => e.CurrentSessionId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserPositionEntity>()
            .HasOne(e => e.SessionUser)
            .WithMany()
            .HasForeignKey(e => e.SessionUserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
