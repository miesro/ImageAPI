using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.DTOs
{
    /// <summary>
    /// DTO for retrieving image metadata
    /// </summary>
    public class GetImageResultDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int Height { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAtUtc { get; set; }
        public List<ImageVariationDto> Variations { get; set; }
    }
}
