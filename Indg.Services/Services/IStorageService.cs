using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Indg.Services.Services
{
    /// <summary>
    /// Defines methods for saving and loading image files from storage.
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves an image to disk at the specified location.
        /// </summary>
        /// <param name="image">The image to save.</param>
        /// <param name="subfolder">The subfolder inside the base storage path where the image should be saved.</param>
        /// <param name="filename">The name of the file (including extension) to save.</param>
        /// <returns>The relative path to the saved image file.</returns>
        Task<string> SaveAsync(Image image, string subfolder, string filename);

        /// <summary>
        /// Loads an image from disk based on its relative path.
        /// </summary>
        /// <param name="relativePath">The relative path to the image file.</param>
        /// <returns>The loaded image from storage.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        /// <exception cref="InvalidImageException">Thrown if the file exists but is not a valid image.</exception>
        Task<Image> LoadAsync(string relativePath);

        /// <summary>
        /// Deletes a file from disk based on its relative path.
        /// </summary>
        /// <param name="path">The relative path to the file to delete.</param>
        /// <returns>A task representing the operation.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
        Task DeleteAsync(string path);
    }
}
