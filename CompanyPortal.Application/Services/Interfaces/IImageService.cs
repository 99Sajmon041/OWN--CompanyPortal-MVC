using Microsoft.AspNetCore.Http;

namespace CompanyPortal.Application.Services.Interfaces;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile imageFile, string folderName, CancellationToken ct);
    void DeleteImage(string imageUrl);
}
