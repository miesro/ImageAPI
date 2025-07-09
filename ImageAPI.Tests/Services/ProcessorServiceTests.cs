using ImageAPI.Tests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;
using Indg.Services.Services;

namespace ImageAPI.Tests.Services
{
    public class ProcessorServiceTests
    {
        private readonly ProcessorService _service;

        public ProcessorServiceTests()
        {
            _service = new ProcessorService();
        }

        [Fact]
        public void ResizeToHeight_ValidInput_RatioShouldBeKept()
        {
            // Arrange
            const int originalWidth = 400;
            const int originalHeight = 800;
            const int targetHeight = 200;

            using var original = new Image<Rgba32>(originalWidth, originalHeight);

            // Act
            using var resized = _service.ResizeToHeight(original, targetHeight);

            // Assert
            Assert.Equal(targetHeight, resized.Height);

            var expectedWidth = (int)(originalWidth * (targetHeight / (double)originalHeight));
            Assert.Equal(expectedWidth, resized.Width);
        }
    }
}
