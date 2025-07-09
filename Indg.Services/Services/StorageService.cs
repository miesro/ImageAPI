using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Indg.Services.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _basePath = "images";

        public async Task<string> SaveAsync(Image image, string subfolder, string filename)
        {
            var path = Path.Combine(_basePath, subfolder, filename);
            var directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            await image.SaveAsync(path);
            var webPath = path.Replace("\\", "/");

            return webPath;
        }

        public async Task<Image> LoadAsync(string relativePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), 
                relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Image not found at path: {relativePath}");

            using var stream = File.OpenRead(fullPath);
            return await Image.LoadAsync(stream);
        }

        public async Task DeleteAsync(string relativePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
            else
            {
                throw new FileNotFoundException($"File at path '{relativePath}' not found.");
            }
        }
    }
}
