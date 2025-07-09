using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Indg.Services.Services
{
    public class ProcessorService : IProcessorService
    {
        public async Task<Image> LoadAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return await Image.LoadAsync(stream);
        }

        public Image ResizeToHeight(Image image, int targetHeight)
        {
            return image.Clone(ctx =>
                ctx.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(0, targetHeight)
                }));
        }
    }
}
