using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;

namespace BikeDeliveryMan.Application.UseCase.Login;

public interface ILoginUseCase
{
    Task<Result<ResponseLogin>> LoginAsync(RequestLoginDeliveryMan request);
}
