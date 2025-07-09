using Indg.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageProcessorDbContext _context;

        public ImageRepository(ImageProcessorDbContext context)
        {
            _context = context;
        }

        public async Task<ImageEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Images
                .Include(i => i.Variations)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(ImageEntity entity)
        {
            await _context.Images.AddAsync(entity);
        }

        public void Remove(ImageEntity entity)
        {
            _context.Images.Remove(entity);
        }

        public async Task AddVariationAsync(ImageVariationEntity variation)
        {
            await _context.ImageVariations.AddAsync(variation);
        }
    }
}
