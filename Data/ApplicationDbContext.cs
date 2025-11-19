using EduMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace EduMaster.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets для всех таблиц
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatParticipant> ChatParticipants { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Role).HasDefaultValue("Student");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsEmailConfirmed).HasDefaultValue(false);
            });

            // Настройка UserProfile
            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfiles");
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Profile)
                    .HasForeignKey<UserProfile>(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Настройка ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.ToTable("ContactMessages");
                entity.HasIndex(e => e.CreatedAt);
                entity.Property(e => e.IsRead).HasDefaultValue(false);
            });

            // Настройка Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Настройка ChatRoom
            modelBuilder.Entity<ChatRoom>(entity =>
            {
                entity.ToTable("ChatRooms");
                entity.Property(e => e.Type).HasDefaultValue("Course");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Настройка ChatParticipant
            modelBuilder.Entity<ChatParticipant>(entity =>
            {
                entity.ToTable("ChatParticipants");
                entity.HasIndex(e => new { e.RoomId, e.UserId }).IsUnique();
            });

            // Настройка ChatMessage
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.ToTable("ChatMessages");
                entity.HasIndex(e => e.RoomId);
                entity.HasIndex(e => e.SenderId);
                entity.HasIndex(e => e.SentAt);
                entity.Property(e => e.MessageType).HasDefaultValue("Text");
                entity.Property(e => e.IsEdited).HasDefaultValue(false);
            });

            // Настройка UserSession
            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.ToTable("UserSessions");
                entity.HasIndex(e => e.SessionToken).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Настройка RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasIndex(e => e.UserId);
                entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            });

            // Ограничения для Role
            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<string>();

            // Ограничения для ChatRoom Type
            modelBuilder.Entity<ChatRoom>()
                .Property(e => e.Type)
                .HasConversion<string>();

            // Ограничения для MessageType
            modelBuilder.Entity<ChatMessage>()
                .Property(e => e.MessageType)
                .HasConversion<string>();
        }
    }
}