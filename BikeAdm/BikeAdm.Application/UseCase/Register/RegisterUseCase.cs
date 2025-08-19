using BikeAdm.Application.Mapping;
using BikeAdm.Communication.Request;
using BikeAdm.Communication.Response;
using BikeAdm.Domain.Repositories.Contracts.User;
using BikeRent.Exception.ExceptionBase;
using Serilog;
using TokenService.Manager.Controller;

namespace BikeAdm.Application.UseCase.Register;

public class RegisterUseCase(
    IUserReadOnly userReadOnlyrepository,
    IUserWriteOnly userWriteOnlyrepository,
    PasswordEncryptor passwordEncryptor,
    ILogger logger) : IRegisterUseCase
{
    private readonly IUserReadOnly _userReadOnlyrepository = userReadOnlyrepository;
    private readonly IUserWriteOnly _userWriteOnlyrepository = userWriteOnlyrepository;
    private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
    private readonly ILogger _logger = logger;

    public async Task<Result<MessageResult>> RegisterUserAsync(RequestRegisterUser request)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Início {nameof(RegisterUserAsync)}.");

            await Validate(request);

            var encryptedPassword = _passwordEncryptor.Encrypt(request.Password);

            var user = request.ToEntity(encryptedPassword);

            await _userWriteOnlyrepository.CreateAsync(user);

            output.Succeeded(new MessageResult("Cadastro realizado com sucesso"));

            _logger.Information($"Fim {nameof(RegisterUserAsync)}.");
        }
        catch (ValidationErrorsException ex)
        {
            var errorMessage = $"Ocorreram erros de validação: {string.Concat(string.Join(", ", ex.ErrorMessages), ".")}";

            _logger.Error(ex, errorMessage);

            output.Failure(ex.ErrorMessages);
        }
        catch (Exception ex)
        {
            var errorMessage = string.Format("Algo deu errado: {0}", ex.Message);

            _logger.Error(ex, errorMessage);

            output.Failure(new List<string>() { errorMessage });
        }

        return output;
    }

    private async Task Validate(RequestRegisterUser request)
    {
        _logger.Information($"Início {nameof(Validate)}.");

        var userValidator = new RegisterValidator();
        var validationResult = userValidator.Validate(request);

        var thereIsWithEmail = await _userReadOnlyrepository.RecoverByEmailAsync(request.Email);

        if (thereIsWithEmail is not null)
            validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ErrorsMessages.EmailAlreadyRegistered));

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}