using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.DTOs
{
    /// <summary>
    /// DTO for representing an image variation.
    /// </summary>
    public class ImageVariationDto
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
