using BikeAdm.Communication.Request;
using BikeAdm.Communication.Response;

namespace BikeAdm.Application.UseCase.Register;

public interface IRegisterUseCase
{
    Task<Result<MessageResult>> RegisterUserAsync(RequestRegisterUser request);
}
