using Chat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.GivenName)
                .HasMaxLength(20)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId);

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(t => t.Id);

            // Connection
            modelBuilder.Entity<Connection>()
                .HasIndex(c => c.ConnectionID);

            // Message
            modelBuilder.Entity<Message>()
                .HasIndex(m => m.Id);
        }
    }
}
