using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.Entities
{
    public class ImageEntity
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int Height { get; set; }
        public DateTime UploadedAtUtc { get; set; }

        public ICollection<ImageVariationEntity> Variations { get; set; }
    }
}
