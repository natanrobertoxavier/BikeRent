using BikeAdm.Communication.Request;
using BikeAdm.Communication.Response;

namespace BikeAdm.Application.UseCase.Login;

public interface ILoginUseCase
{
    Task<Result<ResponseLogin>> LoginAsync(RequestLoginUser request);
}
