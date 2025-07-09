using Indg.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ImageProcessorDbContext _context;

        public IImageRepository ImagesRepository { get; }

        public UnitOfWork(ImageProcessorDbContext context, IImageRepository imageRepository)
        {
            _context = context;
            ImagesRepository = imageRepository;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
