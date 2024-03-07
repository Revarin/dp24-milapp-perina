using Kris.Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Kris.Server.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }
    public DbSet<SessionUserEntity> SessionUsers { get; set; }
    public DbSet<UserPositionEntity> UserPositions { get; set; }
    public DbSet<UserSettingsEntity> UserSettings { get; set; }
    public DbSet<MapPointEntity> MapPoints { get; set; }
    public DbSet<ConversationEntity> Conversations { get; set; }
    public DbSet<MessageEntity> Messages { get; set; }

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
        modelBuilder.Entity<UserSettingsEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<MapPointEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<ConversationEntity>().HasKey(e => e.Id);
        modelBuilder.Entity<MessageEntity>().HasKey(e => e.Id);

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
            .HasForeignKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_0);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_1);
        modelBuilder.Entity<UserPositionEntity>().OwnsOne(e => e.Position_2);

        modelBuilder.Entity<UserSettingsEntity>()
            .HasOne(e => e.User)
            .WithOne(e => e.Settings)
            .HasForeignKey(nameof(UserSettingsEntity), nameof(UserSettingsEntity.Id))
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MapPointEntity>()
            .HasOne(e => e.SessionUser)
            .WithMany(e => e.MapPoints)
            .HasForeignKey(e => e.SessionUserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MapPointEntity>().OwnsOne(e => e.Position);
        modelBuilder.Entity<MapPointEntity>().OwnsOne(e => e.Symbol);

        // OnDelete.NoAction to break cascade cycle, must delete manualy
        modelBuilder.Entity<ConversationEntity>()
            .HasOne(e => e.Session)
            .WithMany(e => e.Conversations)
            .HasForeignKey(e => e.SessionId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<ConversationEntity>()
            .HasMany(e => e.Users)
            .WithMany(e => e.Conversations);

        modelBuilder.Entity<MessageEntity>()
            .HasOne(e => e.Conversation)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<MessageEntity>()
            .HasOne(e => e.Sender)
            .WithMany(e => e.SentMessages)
            .HasForeignKey(e => e.SenderId)
            .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }
}
