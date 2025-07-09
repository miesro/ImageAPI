using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.DTOs
{
    /// <summary>
    /// DTO for uploading an image file.
    /// </summary>
    public class UploadImageDto
    {
        public IFormFile File { get; set; }       
    }
}
