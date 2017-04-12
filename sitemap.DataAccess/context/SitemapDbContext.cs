using System.Data.Entity;

namespace WebSitePerformanceTester.DataAccess.context
{
    public class SitemapDbContext : DbContext
    {
        public SitemapDbContext(): base("WebSitePerformanceTester")
        {
        }

        public DbSet<TestTime> TestsTime { get; set; }
        public DbSet<ResponseTime> ResponseTime { get; set; }
        public DbSet<Domain> Domains { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain>()
              .HasMany(c => c.TestsTime)
              .WithRequired(x=>x.Domain)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<TestTime>()
                .HasRequired(c => c.Domain)
                .WithMany(x=>x.TestsTime);

            modelBuilder.Entity<ResponseTime>()
                .HasRequired(c => c.Domain);

            modelBuilder.Entity<ResponseTime>()
                .HasRequired(c => c.TestsTime);


            base.OnModelCreating(modelBuilder);
        }

    }
}
