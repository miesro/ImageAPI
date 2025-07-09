using Indg.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.Repositories
{
    public interface IImageRepository
    {        
        Task<ImageEntity?> GetByIdAsync(Guid id);
        Task AddAsync(ImageEntity entity);
        void Remove(ImageEntity entity);
        Task AddVariationAsync(ImageVariationEntity variation);
    }
}
