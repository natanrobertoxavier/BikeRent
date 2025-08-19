using BikeDeliveryMan.Application.Mapping;
using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using BikeRent.Exception.ExceptionBase;
using Serilog;
using TokenService.Manager.Controller;

namespace BikeDeliveryMan.Application.UseCase.Login;

public class LoginUseCase(
    IDeliveryManReadOnly deliveryManReadOnlyrepository,
    PasswordEncryptor passwordEncryptor,
    TokenController tokenController,
    ILogger logger) : ILoginUseCase
{
    private readonly IDeliveryManReadOnly _deliveryManReadOnlyrepository = deliveryManReadOnlyrepository;
    private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
    private readonly TokenController _tokenController = tokenController;
    private readonly ILogger _logger = logger;

    public async Task<Result<ResponseLogin>> LoginAsync(RequestLoginDeliveryMan request)
    {
        var output = new Result<ResponseLogin>();

        try
        {
            _logger.Information($"Início {nameof(LoginAsync)}.");

            var encryptedPassword = _passwordEncryptor.Encrypt(request.Password);

            var entity = await _deliveryManReadOnlyrepository.RecoverByEmailPasswordAsync(request.Email.ToLower(), encryptedPassword);

            if (entity is null)
            {
                _logger.Information($"Fim {nameof(LoginAsync)}. Não foram encontrados dados.");

                throw new InvalidLoginException();
            }
            else
            {
                var token = _tokenController.GenerateToken(entity.Email);

                output.Succeeded(entity.ToResponseLogin(token));

                _logger.Information($"Fim {nameof(LoginAsync)}.");
            }
        }
        catch (InvalidLoginException ex)
        {
            output.Failure(new List<string>() { ex.Message });
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("Algo deu errado: {0}", ex.Message);

            output.Failure(new List<string>() { errorMessage });

            _logger.Error(ex, errorMessage);
        }

        return output;
    }
}
