using Indg.DataAccess.Entities;
using Indg.DataAccess.UnitOfWork;
using Indg.Services.DTOs;
using Indg.Services.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using Xunit;
using Indg.Services.Exceptions;
using ImageAPI.Tests.Helpers;

namespace ImageAPI.Tests.Services
{
    public class ImageServiceTests
    {
        private readonly IStorageService _storage;
        private readonly IProcessorService _processor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ImageService> _logger;
        private readonly ImageService _service;

        public ImageServiceTests()
        {
            _storage = Substitute.For<IStorageService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _logger = Substitute.For<ILogger<ImageService>>();
            _processor = Substitute.For<IProcessorService>();

            _service = new ImageService(_unitOfWork, _storage, _processor);
        }

        [Fact]
        public async Task UploadImageAsync_ValidInput_ShouldReturnExpectedResult()
        {
            // Arrange
            const string fileName = "test.jpg";
            const string contentType = "image/jpeg";
            const int thumbnailHeight = 160;
            const string originalsFolder = "originals";
            const string thumbnailsFolder = "variations";
            const string expectedOriginalPath = "images/originals/test.jpg";
            const string expectedThumbnailPath = "images/thumbnails/test_thumb.jpg";

            var formFile = ImageHelper.CreateFakeFormFile(fileName, contentType);

            var originalImage = new Image<Rgba32>(100, 200);
            var thumbnailImage = new Image<Rgba32>(80, thumbnailHeight);

            _processor.LoadAsync(formFile).Returns(originalImage);
            _processor.ResizeToHeight(originalImage, thumbnailHeight).Returns(thumbnailImage);

            _storage.SaveAsync(originalImage, originalsFolder, Arg.Any<string>())
                .Returns(expectedOriginalPath);

            _storage.SaveAsync(thumbnailImage, thumbnailsFolder, Arg.Any<string>())
                .Returns(expectedThumbnailPath);

            var dto = new UploadImageDto { File = formFile };

            // Act
            var result = await _service.UploadImageAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOriginalPath, result.OriginalPath);
            Assert.Equal(expectedThumbnailPath, result.ThumbnailPath);

            await _unitOfWork.ImagesRepository.Received(1).AddAsync(Arg.Is<ImageEntity>(e =>
                e.Path == expectedOriginalPath &&
                e.Variations.Count == 1 &&
                e.Variations.First().Path == expectedThumbnailPath));

            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task UploadImageAsync_InvalidExtension_ShouldThrowImageException()
        {
            // Arrange
            var formFile = ImageHelper.CreateFakeFormFile("bad.txt", "text/plain");
            var dto = new UploadImageDto { File = formFile };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidImageException>(() =>
                _service.UploadImageAsync(dto));
        }

        [Fact]
        public async Task DeleteImageAsync_ImageDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _unitOfWork.ImagesRepository.GetByIdAsync(id).Returns((ImageEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteImageAsync(id));
        }

        [Fact]
        public async Task DeleteImageAsync_ImageExists_ShouldCallStorageToRemove()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var variationId = Guid.NewGuid();
            var originalPath = "images/originals/image.jpg";
            var variationPath = "images/variations/thumb.jpg";

            var imageEntity = new ImageEntity
            {
                Id = imageId,
                Path = originalPath,
                FileName = "image.jpg",
                Height = 300,
                UploadedAtUtc = DateTime.UtcNow,
                Variations = new List<ImageVariationEntity>
                {
                    new ImageVariationEntity
                    {
                        Id = variationId,
                        Path = variationPath,
                        Height = 160,
                        CreatedAtUtc = DateTime.UtcNow
                    }
                }
            };

            _unitOfWork.ImagesRepository.GetByIdAsync(imageId).Returns(imageEntity);

            // Act
            await _service.DeleteImageAsync(imageId);

            // Assert
            await _storage.Received(1).DeleteAsync(originalPath);
            await _storage.Received(1).DeleteAsync(variationPath);
            _unitOfWork.ImagesRepository.Received(1).Remove(imageEntity);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}
