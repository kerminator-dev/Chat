using Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<HubConnection> Connections { get; set; }
        public DbSet<DialogueMessage> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Dialogue> Dialogues { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(20)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id);

          // modelBuilder.Entity<User>()
          //     .HasMany(u => u.CreatorDialogues)
          //     .WithOne(d => d.Creator)
          //     .OnDelete(DeleteBehavior.Restrict);

           // modelBuilder.Entity<User>()
           //     .HasMany(u => u.MemberDialogues)
           //     .WithOne(d => d.MemberUserId)
           //     .OnDelete(DeleteBehavior.Restrict);

            // RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasIndex(t => t.Id)
                .IsUnique();

            modelBuilder.Entity<RefreshToken>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            // Connection
            modelBuilder.Entity<HubConnection>()
                .HasIndex(c => new { c.UserId, c.Id } );

            // Message
            modelBuilder.Entity<DialogueMessage>()
                .HasIndex(m => m.Id);

            // Conversation
            modelBuilder.Entity<Conversation>()
                .HasIndex(c => c.Id)
                .IsUnique();

            modelBuilder.Entity<Conversation>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            // Dialogues
            modelBuilder.Entity<Dialogue>()
                .HasIndex(d => d.Id)
                .IsUnique();

            modelBuilder.Entity<Dialogue>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd();

         

          //  modelBuilder.Entity<Dialogue>()
          //      .Property(d => d.Creator)
          //      .IsRequired(true)
          //     
          //      .HasMany(d => d.Creator.CreatorDialogues);
          //
          //  modelBuilder.Entity<Dialogue>()
          //      .HasMany(d => d.Member.MemberDialogues);
          //   
        }
    }
}
