using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(u => u.GivenName)
                .HasMaxLength(20)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId);

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(t => t.RefreshTokenId)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.RefreshTokenId)
                .ValueGeneratedOnAdd();

            // Connection
            modelBuilder.Entity<Connection>()
                .HasIndex(c => new { c.UserId, c.ConnectionId } );

            // Message
            modelBuilder.Entity<Message>()
                .HasIndex(m => m.MessageId);

            // Conversation
            modelBuilder.Entity<Conversation>()
                .HasIndex(c => c.ConversationId)
                .IsUnique();

            modelBuilder.Entity<Conversation>()
                .Property(c => c.ConversationId)
                .ValueGeneratedOnAdd();
        }
    }
}
