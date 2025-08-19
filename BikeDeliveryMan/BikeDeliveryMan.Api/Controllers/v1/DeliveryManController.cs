using BikeDeliveryMan.Api.Filters;
using BikeDeliveryMan.Application.UseCase.Register;
using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace BikeDeliveryMan.Api.Controllers.v1;

public class DeliveryManController : BikeDeliveryManController
{
    [HttpPost]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDeliveryManAsync(
        [FromServices] IRegisterUseCase useCase,
        [FromBody] RegisterDeliveryMan request)
    {
        var result = await useCase.RegisterDeliveryManAsync(request);

        return ResponseCreate(result);
    }

    [HttpPost("send-photo")]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<MessageResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendPhotoAsync(
        [FromServices] IRegisterUseCase useCase,
        [FromBody] RegisterDeliveryMan request)
    {
        var result = await useCase.RegisterDeliveryManAsync(request);

        return ResponseCreate(result);
    }
}
