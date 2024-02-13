using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }
    public DbSet<SessionUserEntity> SessionUsers { get; set; }
    public DbSet<UserPositionEntity> UserPositions { get; set; }
    public DbSet<MapPointEntity> MapPoints { get; set; }

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
        modelBuilder.Entity<UserEntity>()
            .HasMany(e => e.AllSessions)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

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
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<SessionUserEntity>()
            .HasOne(e => e.User)
            .WithMany(e => e.AllSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<SessionUserEntity>()
            .HasMany(e => e.MapPoints)
            .WithOne(e => e.SessionUser)
            .HasForeignKey(e => new { e.UserId, e.SessionId })
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntity>()
            .HasOne(e => e.CurrentSession)
            .WithOne()
            .HasForeignKey(typeof(UserEntity), nameof(UserEntity.Id), nameof(UserEntity.CurrentSessionId))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserPositionEntity>().HasKey(e => new { e.UserId, e.SessionId });
        modelBuilder.Entity<UserPositionEntity>()
            .HasOne(e => e.SessionUser)
            .WithOne()
            .HasForeignKey(typeof(UserPositionEntity), nameof(UserPositionEntity.UserId), nameof(UserPositionEntity.SessionId))
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_0);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_1);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_2);

        modelBuilder.Entity<MapPointEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<MapPointEntity>()
            .HasOne(e => e.SessionUser)
            .WithMany(e => e.MapPoints)
            .HasForeignKey(e => new { e.UserId, e.SessionId })
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<MapPointEntity>().OwnsOne(e => e.Position);
        modelBuilder.Entity<MapPointEntity>().OwnsOne(e => e.Symbol);

        base.OnModelCreating(modelBuilder);
    }
}
