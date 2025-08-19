using Microsoft.AspNetCore.Http;

namespace BikeDeliveryMan.Domain.Services;
public interface IPhotoStorageService
{
    Task SavePhotoAsync(string fileName, IFormFile file);
}
