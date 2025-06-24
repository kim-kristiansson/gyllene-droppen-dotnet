using GylleneDroppen.Application.Interfaces.Services;
using GylleneDroppen.Application.Services;

namespace GylleneDroppen.Infrastructure.Services;

public class ImageService(IAppEnvironment appEnvironment) : IImageService
{
    private readonly string[] _allowedImageTypes =
        ["image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp"];

    public async Task<string> SaveImageAsync(Stream imageStream, string fileName, string folder = "whiskies")
    {
        var uploadsFolder = Path.Combine(appEnvironment.WebRootPath, "images", folder);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileExtension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream);

        return $"/images/{folder}/{uniqueFileName}";
    }

    public Task<bool> DeleteImageAsync(string imagePath)
    {
        try
        {
            if (string.IsNullOrEmpty(imagePath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(appEnvironment.WebRootPath, imagePath.TrimStart('/'));

            if (!File.Exists(fullPath)) return Task.FromResult(false);
            File.Delete(fullPath);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
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