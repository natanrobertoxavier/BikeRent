using BikeDeliveryMan.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BikeDeliveryMan.Infrastructure.Services;

public class PhotoStorageService : IPhotoStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _storagePath;

    public PhotoStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        _storagePath = _configuration["StoragePath"] ?? throw new ArgumentException("Upload path not configured");

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
