using PicBot.Domain.ImageService;

namespace PicBot.Application.Abstractions;

public interface IImageService
{
    Task<List<ImageResponse>> GetImageByTextAsync(string text, CancellationToken cancellationToken);
}