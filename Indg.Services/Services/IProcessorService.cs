using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Indg.Services.Services
{
    /// <summary>
    /// Provides image processing operations such as loading and resizing.
    /// </summary>
    public interface IProcessorService
    {
        /// <summary>
        /// Loads an image from the uploaded form file into memory.
        /// </summary>
        /// <param name="file">The uploaded image file.</param>
        /// <returns>An image object representing the uploaded file.</returns>
        Task<Image> LoadAsync(IFormFile file);

        /// <summary>
        /// Creates a resized clone of the image with the specified height, maintaining aspect ratio.
        /// </summary>
        /// <param name="original">The original image to resize.</param>
        /// <param name="targetHeight">The desired height for the resized image.</param>
        /// <returns>A new image resized to the specified height.</returns>
        Image ResizeToHeight(Image original, int targetHeight);
    }
}
