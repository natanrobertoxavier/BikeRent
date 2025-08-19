using BikeDeliveryMan.Communication.Response;
using Microsoft.AspNetCore.Http;

namespace BikeDeliveryMan.Application.UseCase.SendPhoto;

public interface ISendPhotoUseCase
{
    Task<Result<MessageResult>> SendPhotoAsync(IFormFile request);
}
