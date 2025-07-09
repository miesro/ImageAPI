using ImageAPI.Tests.Helpers;
using Indg.Services.DTOs;
using Indg.Services.Exceptions;
using Indg.Services.Services;
using IndgImageProcessor.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ImageAPI.Tests.Controllers
{
    public class ImagesControllerTests
    {
        private readonly IImageService _imageService;
        private readonly ILogger<ImagesController> _logger;
        private readonly ImagesController _controller;

        public ImagesControllerTests()
        {
            _imageService = Substitute.For<IImageService>();
            _logger = Substitute.For<ILogger<ImagesController>>();
            _controller = new ImagesController(_logger, _imageService);
        }

        [Fact]
        public async Task UploadImage_ValidDto_ShouldReturnOkWithResult()
        {
            // Arrange
            var dto = new UploadImageDto
            {
                File = ImageHelper.CreateFakeFormFile()
            };

            var resultDto = new UploadImageResultDto
            {
                Id = Guid.NewGuid(),
                OriginalPath = "images/originals/test.jpg",
                ThumbnailPath = "images/thumbnails/test_thumb.jpg"
            };

            _imageService.UploadImageAsync(dto).Returns(resultDto);

            // Act
            var result = await _controller.UploadImage(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Same(resultDto, okResult.Value);
        }

        [Fact]
        public async Task UploadImage_InvalidDto_ShouldReturnBadRequest()
        {
            // Arrange
            const string fileName = "bad.txt";
            const string contentType = "text/plain";
            const string expectedMessage = "Unsupported format";
            const int expectedStatusCode = 400;

            var formFile = ImageHelper.CreateFakeFormFile(fileName, contentType);

            var dto = new UploadImageDto
            {
                File = formFile
            };

            _imageService.UploadImageAsync(dto).Throws(new InvalidImageException(expectedMessage));

            // Act
            var result = await _controller.UploadImage(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedStatusCode, badRequest.StatusCode);
            Assert.Equal(expectedMessage, badRequest.Value);
        }
    }
}
