using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NewsManagementService.Models;

namespace NewsManagementService.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<NewsSummary> NewsSummaries { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<MacroNewsCategory> MacroNewsCategories { get; set; }
        public DbSet<UserPreferencesReplica> UserPreferencesReplicas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsSummary>()
                .HasMany(e => e.NewsCategories)
                .WithMany(e => e.NewsSummaries);
        }
    }
}
