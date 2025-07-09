using Indg.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IImageRepository ImagesRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
