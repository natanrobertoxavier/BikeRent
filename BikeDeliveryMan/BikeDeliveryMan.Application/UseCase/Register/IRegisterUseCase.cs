using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;

namespace BikeDeliveryMan.Application.UseCase.Register;

public interface IRegisterUseCase
{
    Task<Result<MessageResult>> RegisterDeliveryManAsync(RegisterDeliveryMan request);
}
