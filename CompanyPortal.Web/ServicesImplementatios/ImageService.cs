using CompanyPortal.Application.Services.Interfaces;

namespace CompanyPortal.Web.ServicesImplementations;

public sealed class ImageService(IWebHostEnvironment environment) : IImageService
{
    public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName, CancellationToken ct)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            throw new Exception("Soubor nebyl vybrán.");
        }

        string uploadsFolder = Path.Combine(environment.WebRootPath, "images", folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string extension = Path.GetExtension(imageFile.FileName);
        string fileName = $"{Guid.NewGuid()}{extension}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        await using FileStream stream = new(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream, ct);

        return $"/images/{folderName}/{fileName}";
    }

    public void DeleteImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return;
        }

        string relativePath = imageUrl.TrimStart('/');
        string filePath = Path.Combine(environment.WebRootPath, relativePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}