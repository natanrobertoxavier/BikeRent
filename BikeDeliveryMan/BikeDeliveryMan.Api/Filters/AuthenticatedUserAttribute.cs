using BikeRent.Exception.ExceptionBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenService.Manager.Controller;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using System.ComponentModel.DataAnnotations;

namespace BikeDeliveryMan.Api.Filters;

public class AuthenticatedUserAttribute(
    TokenController tokenController,
    IDeliveryManReadOnly deliveryManReadOnlyrepository) : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController = tokenController;
    private readonly IDeliveryManReadOnly _deliveryManReadOnlyrepository = deliveryManReadOnlyrepository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenInRequest(context);
            var userEmail = _tokenController.RecoverEmail(token);

            var user = await _deliveryManReadOnlyrepository.RecoverByEmailAsync(userEmail) ?? throw new ValidationException("Usuário não localizado");
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch
        {
            UserWithoutPermission(context);
        }
    }

    private static string TokenInRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new BikeRentException("Usuário não autenticado");
        }

        return authorization["Bearer".Length..].Trim();
    }

    private static void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new Communication.Response.ResponseError(ErrorsMessages.ExpiredToken));
    }

    private static void UserWithoutPermission(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new Communication.Response.ResponseError(ErrorsMessages.UserWithoutPermission));
    }
}
