using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.DataAccess.Entities
{
    public class ImageVariationEntity
    {
        public Guid Id { get; set; }
        public Guid ImageId { get; set; }
        public string Path { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public ImageEntity Image { get; set; }
    }
}
