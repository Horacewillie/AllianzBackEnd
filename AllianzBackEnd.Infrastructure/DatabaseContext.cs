using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AllianzBackEnd.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    }
}