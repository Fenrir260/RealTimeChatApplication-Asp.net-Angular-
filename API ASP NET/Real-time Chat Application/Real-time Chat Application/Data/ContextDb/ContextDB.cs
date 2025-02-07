using Microsoft.EntityFrameworkCore;
using Real_time_Chat_Application.Data.Entity;

namespace Real_time_Chat_Application.Data.ContextDb
{
    public class ContextDB : DbContext
    {
        public DbSet<User> UsersDB { get; set; }
        public DbSet<Chat> ChatDB { get; set; }
        public DbSet<Message> MessagesDB { get; set; }
        public DbSet<Friendship> FriendshipsDB { get; set; }
        public DbSet<UserChat> UserChatDB { get; set; }
        public DbSet<UserProfile> UserProfilesDB { get; set; }

        public ContextDB(DbContextOptions<ContextDB> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChat>()
                .HasKey(uc => new { uc.UserId, uc.ChatId });

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UserChats)
                .HasForeignKey(uc => uc.ChatId);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany(u => u.Friends)  
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany(u => u.FriendOf)
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.UserId);

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne(static u => u.UserProfile)
                .HasForeignKey<UserProfile>(up => up.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
