using BikeDeliveryMan.Domain.Entities;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using Microsoft.AspNetCore.Http;
using TokenService.Manager.Controller;

namespace BikeDeliveryMan.Application.Services;

public class LoggedUser(
    IHttpContextAccessor httpContextAccessor,
    TokenController tokenController,
    IDeliveryManReadOnly repository) : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly TokenController _tokenController = tokenController;
    private readonly IDeliveryManReadOnly _repository = repository;

    public async Task<DeliveryMan> GetLoggedUserAsync()
    {
        var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var email = _tokenController.RecoverEmail(token);

        return await _repository.RecoverByEmailAsync(email);
    }
}
