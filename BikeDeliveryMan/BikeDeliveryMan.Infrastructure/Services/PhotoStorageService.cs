using BikeDeliveryMan.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace BikeDeliveryMan.Infrastructure.Services;

public class PhotoStorageService : IPhotoStorageService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Photos");

    public PhotoStorageService()
    {
        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task SavePhotoAsync(string fileName, IFormFile file)
    {
        var filePath = Path.Combine(_storagePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
    }
}
