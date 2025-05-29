namespace GylleneDroppen.Application.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(Stream imageStream, string fileName, string folder = "whiskies");
    Task<bool> DeleteImageAsync(string imagePath);
    bool IsValidImageType(string contentType);
    bool IsValidImageSize(long fileSize, long maxSizeInBytes = 5_000_000); // 5MB default
}