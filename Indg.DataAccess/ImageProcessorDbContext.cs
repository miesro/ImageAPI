using Indg.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Indg.DataAccess
{
    public class ImageProcessorDbContext : DbContext
    {
        public ImageProcessorDbContext(DbContextOptions<ImageProcessorDbContext> options)
        : base(options)
        {

        }

        public DbSet<ImageEntity> Images => Set<ImageEntity>();
        public DbSet<ImageVariationEntity> ImageVariations => Set<ImageVariationEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageEntity>()
                .HasMany(i => i.Variations)
                .WithOne(v => v.Image)
                .HasForeignKey(v => v.ImageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
