using Indg.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indg.Services.Services
{
    /// <summary>
    /// Interface for handling image operations.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Validates the uploaded image file, stores the original and a thumbnail, and saves metadata.
        /// </summary>
        /// <param name="uploadImage">The DTO containing the image file to upload.</param>
        /// <returns>A DTO with information about the stored image and its thumbnail.</returns>
        /// <exception cref="InvalidImageException">
        /// Thrown when the file is missing, empty, has an unsupported format, or is not a valid image.
        /// </exception>
        Task<UploadImageResultDto> UploadImageAsync(UploadImageDto uploadImage);

        /// <summary>
        /// Retrieves metadata of a stored image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the image.</param>
        /// <returns>A DTO containing image metadata.</returns>
        /// <exception cref="NotFoundException">Thrown when the image with the specified ID is not found.</exception>
        Task<GetImageResultDto> GetImageMetadataAsync(Guid id);

        /// <summary>
        /// Deletes a stored image and all associated data by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the image to delete.</param>
        /// <exception cref="NotFoundException">Thrown if the image does not exist.</exception>
        Task DeleteImageAsync(Guid id);

        /// <summary>
        /// Retrieves or generates a resized variation of the image with the specified height.
        /// </summary>
        /// <param name="id">The ID of the original image.</param>
        /// <param name="height">The target height for the resized variation.</param>
        /// <returns>The path to the generated or cached image variation.</returns>
        /// <exception cref="NotFoundException">Thrown if the image is not found.</exception>
        /// <exception cref="InvalidHeightException">Thrown if the requested height exceeds the original image height.</exception>
        Task<string> GetImageVariationAsync(Guid id, int height);
    }
}
