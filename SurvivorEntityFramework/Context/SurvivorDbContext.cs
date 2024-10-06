using Microsoft.EntityFrameworkCore;
using SurvivorEntityFramework.Entities;

namespace SurvivorEntityFramework.Context
{
    public class SurvivorDbContext: DbContext
    {
        public SurvivorDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();

        public DbSet<CompetitorEntity> Competitors => Set<CompetitorEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CategoryEntity>(entity =>
            {
                entity.ToTable("Category");
                entity.Property(entity => entity.Name).IsRequired();
            
            
                entity.HasMany(c => c.Competitors)
                      .WithOne(c => c.Category)
                      .HasForeignKey(c => c.CategoryId);
            });

            modelBuilder.Entity<CompetitorEntity>(entity =>
            {
                entity.ToTable("Competitor");
                entity.Property(x => x.CategoryId).IsRequired();
                entity.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            });

        }



    }
}
