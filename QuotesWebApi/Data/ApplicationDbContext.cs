using Microsoft.EntityFrameworkCore;
using QuotesWebApi.Models;


namespace QuotesWebApi.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public void SeedData()
        {
            // Check if there are already data to avoid re-seeding
            if (!Tags.Any())
            {
                var defaultTags = new List<Tag>
            {
                new Tag { Name = "Inspirational" },
                new Tag { Name = "Life" },
                new Tag { Name = "Love" },
                new Tag { Name = "Humor" },
                new Tag { Name = "Wisdom" },
                new Tag { Name = "Motivational" },
                new Tag { Name = "Happiness" },
                new Tag { Name = "Philosophy" },
            };

                Tags.AddRange(defaultTags);

                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
