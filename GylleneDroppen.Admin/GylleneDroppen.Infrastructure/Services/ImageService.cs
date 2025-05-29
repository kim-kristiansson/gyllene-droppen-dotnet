using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;
using Microsoft.AspNetCore.Hosting;

namespace GylleneDroppen.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    private readonly string[] _allowedImageTypes =
        { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveImageAsync(Stream imageStream, string fileName, string folder = "whiskies")
    {
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);

        // Create directory if it doesn't exist
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generate unique filename
        var fileExtension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save the image
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream);

        // Return relative path for storing in database
        return $"/images/{folder}/{uniqueFileName}";
    }

    public async Task<bool> DeleteImageAsync(string imagePath)
    {
        try
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool IsValidImageType(string contentType)
    {
        return _allowedImageTypes.Contains(contentType.ToLowerInvariant());
    }

    public bool IsValidImageSize(long fileSize, long maxSizeInBytes = 5_000_000)
    {
        return fileSize > 0 && fileSize <= maxSizeInBytes;
    }
}