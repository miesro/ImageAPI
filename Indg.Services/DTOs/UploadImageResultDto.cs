using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.DTOs
{
    /// <summary>
    /// DTO for the result of an image upload operation.
    /// </summary>
    public class UploadImageResultDto
    {
        public Guid Id { get; set; }
        public string OriginalPath { get; set; }
        public string ThumbnailPath { get; set; }
    }
}
