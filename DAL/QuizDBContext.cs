using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class QuizDBContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public QuizDBContext(DbContextOptions<QuizDBContext> options) : base(options){}
        public QuizDBContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = 172.16.18.17\\SQLEXPRESS01; Database = QuizApp; user Id = quizapp; Password = Quiz@1; TrustServerCertificate = true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Organisation)
                .WithMany()
                .HasForeignKey(u => u.OrganisationId);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<User>()
                .Property(u => u.IsApproved)
                .HasDefaultValue(false);
            modelBuilder.Entity<User>()
                .Property(u => u.RoleId)
                .HasDefaultValue(3);
        }

    }
}
