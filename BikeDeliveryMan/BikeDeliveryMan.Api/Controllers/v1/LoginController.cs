using BikeDeliveryMan.Application.UseCase.Login;
using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BikeDeliveryMan.Api.Controllers.v1;

public class LoginController : BikeDeliveryManController
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<ResponseLogin>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RecoverByEmailPasswordAsync(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequestLoginDeliveryMan request)
    {
        var result = await useCase.LoginAsync(request);

        return Response(
            result,
            HttpStatusCode.OK,
            HttpStatusCode.Unauthorized,
            HttpStatusCode.Unauthorized);
    }
}
