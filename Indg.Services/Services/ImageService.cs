using Indg.DataAccess.Entities;
using Indg.DataAccess.UnitOfWork;
using Indg.Services.DTOs;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Indg.Services.Exceptions;

namespace Indg.Services.Services
{
    public class ImageService : IImageService
    {
        private static readonly int _thumbnailHeight = 160;
        private static readonly string _originalsFolderName = "originals";
        private static readonly string _variationsFolderName = "variations";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;
        private readonly IProcessorService _processorService;

        public ImageService(
            IUnitOfWork unitOfWork, 
            IStorageService imageStorageService,
            IProcessorService imageProcessorService)
        {
            _unitOfWork = unitOfWork;
            _storageService = imageStorageService;
            _processorService = imageProcessorService;

        }

        public async Task<UploadImageResultDto> UploadImageAsync(UploadImageDto uploadImage)
        {
            //validate input image and load it in memory
            var originalImage = await ValidateAndLoadImageAsync(uploadImage.File);

            //store original image
            var originalId = Guid.NewGuid();
            var fileExtension = Path.GetExtension(uploadImage.File.FileName);
            var utcNow = DateTime.UtcNow;

            var originalPath = await _storageService.SaveAsync(
                originalImage, _originalsFolderName, $"{originalId}{fileExtension}");

            //create and store thumbnail variation
            var thumbnailImage = _processorService.ResizeToHeight(originalImage, _thumbnailHeight);
            var thumbnailId = Guid.NewGuid();
            var thumbnailPath = await _storageService.SaveAsync(
                thumbnailImage, _variationsFolderName, $"{thumbnailId}{fileExtension}");

            //store images metadata in the database
            var entity = new ImageEntity
            {
                Id = originalId,
                FileName = uploadImage.File.FileName,
                Path = originalPath,
                Height = originalImage.Height,
                UploadedAtUtc = utcNow,
                Variations = new List<ImageVariationEntity>
                {
                    new ImageVariationEntity
                    {
                        Id = thumbnailId,
                        Path = thumbnailPath,
                        Height = thumbnailImage.Height,
                        CreatedAtUtc = utcNow
                    }
                }
            };

            await _unitOfWork.ImagesRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new UploadImageResultDto
            {
                Id = originalId,
                OriginalPath = originalPath,
                ThumbnailPath = thumbnailPath
            };
        }

        public async Task<GetImageResultDto> GetImageMetadataAsync(Guid id)
        {
            var entity = await _unitOfWork.ImagesRepository.GetByIdAsync(id);

            if (entity == null)
                throw new NotFoundException($"Image with ID {id} was not found.");

            return new GetImageResultDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                Height = entity.Height,
                Path = entity.Path,
                UploadedAtUtc = entity.UploadedAtUtc,
                Variations = entity.Variations.Select(v => new ImageVariationDto
                {
                    Id = v.Id,
                    Path = v.Path,
                    Height = v.Height,
                    CreatedAtUtc = v.CreatedAtUtc
                }).ToList()
            };
        }

        public async Task DeleteImageAsync(Guid id)
        {
            var entity = await _unitOfWork.ImagesRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException();

            await DeleteImageAndVariationsAsync(entity);

            _unitOfWork.ImagesRepository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetImageVariationAsync(Guid id, int height)
        {
            var entity = await _unitOfWork.ImagesRepository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException("Image not found");

            if (height > entity.Height)
                throw new InvalidHeightException("Requested height exceeds original image height");

            // check if the variation already exists
            var existingVariation = entity.Variations.FirstOrDefault(v => v.Height == height);
            if (existingVariation != null)
                return existingVariation.Path;

            // create and save the variation
            var originalImage = await _storageService.LoadAsync(entity.Path);
            var resizedImage = _processorService.ResizeToHeight(originalImage, height);

            var variationId = Guid.NewGuid();
            var fileExtension = Path.GetExtension(entity.FileName);
            var variationPath = await _storageService.SaveAsync(
                resizedImage,
                _variationsFolderName,
                $"{variationId}{fileExtension}");

            var newVariation = new ImageVariationEntity
            {
                Id = variationId,
                Height = height,
                Path = variationPath,
                CreatedAtUtc = DateTime.UtcNow,
                Image = entity // ✅ correctly sets the navigation + foreign key
            };

            await _unitOfWork.ImagesRepository.AddVariationAsync(newVariation);
            await _unitOfWork.SaveChangesAsync();

            return variationPath;
        }

        private async Task<Image> ValidateAndLoadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidImageException("File is not provided or empty");

            if (!IsImageFile(file))
                throw new InvalidImageException("Unsupported image format");

            try
            {
                return await _processorService.LoadAsync(file);
            }
            catch
            {
                throw new InvalidImageException("File is not a valid image");
            }
        }

        private bool IsImageFile(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
            return allowedTypes.Contains(file.ContentType.ToLower());
        }

        private async Task DeleteImageAndVariationsAsync(ImageEntity image)
        {
            foreach (var variation in image.Variations)
            {
                await _storageService.DeleteAsync(variation.Path);
            }

            await _storageService.DeleteAsync(image.Path);
        }
    }
}
