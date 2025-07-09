using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAPI.Tests.Helpers
{
    public static class ImageHelper
    {
        public static IFormFile CreateFakeFormFile(string fileName = "test.jpg", string contentType = "image/jpeg")
        {
            var content = new byte[] { 1, 2, 3 };
            var stream = new MemoryStream(content);
            return new FormFile(stream, 0, content.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
