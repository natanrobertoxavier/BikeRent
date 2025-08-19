using BikeAdm.Application.UseCase.Login;
using BikeAdm.Communication.Request;
using BikeAdm.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BikeAdm.Api.Controllers.v1;

public class LoginController : BikeAdmController
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RecoverByCRPasswordAsync(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequestLoginUser request)
    {
        var result = await useCase.LoginAsync(request);

        return Response(
            result,
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Unauthorized);
    }
}
