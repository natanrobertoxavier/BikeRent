using BikeAdm.Application.UseCase.Register;
using BikeAdm.Communication.Request;
using BikeAdm.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace BikeAdm.Api.Controllers.v1;

public class UserController : BikeAdmController
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUserAsync(
        [FromServices] IRegisterUseCase useCase,
        [FromBody] RequestRegisterUser request)
    {
        var result = await useCase.RegisterUserAsync(request);

        return ResponseCreate(result);
    }
}
