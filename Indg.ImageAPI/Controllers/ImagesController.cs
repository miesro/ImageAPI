using Indg.Services.DTOs;
using Indg.Services.Exceptions;
using Indg.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndgImageProcessor.Controllers;

/// <summary>
/// Controller for managing image uploads, retrievals, deletions, and variations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ILogger<ImagesController> _logger;
    private readonly IImageService _imageService;

    public ImagesController(ILogger<ImagesController> logger, IImageService imageService)
    {
        _logger = logger;
        _imageService = imageService;
    }

    /// <summary>
    /// Uploads a new image, generates a thumbnail, and stores metadata.
    /// </summary>
    /// <param name="dto">The image file.</param>
    /// <returns>Returns metadata of the uploaded image or a BadRequest if the image is invalid.</returns>
    /// <response code="200">Image successfully uploaded.</response>
    /// <response code="400">Invalid image format or data.</response>
    [HttpPost]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto)
    {
        try
        {
            var result = await _imageService.UploadImageAsync(dto);
            return Ok(result);
        }
        catch (InvalidImageException ex)
        {
            _logger.LogWarning(ex, $"Invalid image upload attempt: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves metadata of an image by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the image.</param>
    /// <returns>Returns the image metadata or NotFound if the image does not exist.</returns>
    /// <response code="200">Metadata retrieved successfully.</response>
    /// <response code="404">Image with the given ID was not found.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(Guid id)
    {
        try
        {
            var image = await _imageService.GetImageMetadataAsync(id);
            return Ok(image);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Image with ID {ImageId} was not found", id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Deletes an image and its associated data by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the image to delete.</param>
    /// <returns>NoContent if deletion was successful or NotFound if the image does not exist.</returns>
    /// <response code="204">Image deleted successfully.</response>
    /// <response code="404">Image with the given ID was not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        try
        {
            await _imageService.DeleteImageAsync(id);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, $"Image with ID {id} not found for deletion");
            return NotFound($"Image with ID {id} was not found");
        }

        return NoContent();
    }

    /// <summary>
    /// Gets a resized variation of an image at the specified height.
    /// </summary>
    /// <param name="id">The ID of the image to resize.</param>
    /// <param name="height">The target height for the variation.</param>
    /// <returns>Path to the resized image variation.</returns>
    /// <response code="200">Variation generated or retrieved successfully.</response>
    /// <response code="400">The provided height is invalid.</response>
    /// <response code="404">Image with the given ID was not found.</response>
    [HttpGet("{id}/variations")]
    public async Task<IActionResult> GetImageVariation(Guid id, [FromQuery] int height)
    {
        if (height <= 0) 
        {
            _logger.LogWarning("Provided height wasn't a positive number");
            return BadRequest("Height must be a positive number");
        }  

        try
        {
            var variationPath = await _imageService.GetImageVariationAsync(id, height);
            return Ok(new { VariationPath = variationPath });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, $"Image {id} not found");
            return NotFound(ex.Message);
        }
        catch (InvalidHeightException ex)
        {
            _logger.LogWarning(ex, $"Invalid height {height} requested for image {id}");
            return BadRequest(ex.Message);
        }
    }
}
