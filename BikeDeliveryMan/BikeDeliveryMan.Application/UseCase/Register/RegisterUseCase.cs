using BikeDeliveryMan.Application.Mapping;
using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Communication.Response;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using BikeRent.Exception.ExceptionBase;
using Serilog;
using TokenService.Manager.Controller;

namespace BikeDeliveryMan.Application.UseCase.Register;

public class RegisterUseCase(
    IDeliveryManReadOnly deliveryManReadOnlyrepository,
    IDeliveryManWriteOnly deliveryManWriteOnlyrepository,
    PasswordEncryptor passwordEncryptor,
    ILogger logger) : IRegisterUseCase
{
    private readonly IDeliveryManReadOnly _deliveryManReadOnlyrepository = deliveryManReadOnlyrepository;
    private readonly IDeliveryManWriteOnly _deliveryManWriteOnlyrepository = deliveryManWriteOnlyrepository;
    private readonly PasswordEncryptor _passwordEncryptor = passwordEncryptor;
    private readonly ILogger _logger = logger;

    public async Task<Result<MessageResult>> RegisterDeliveryManAsync(RegisterDeliveryMan request)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Início {nameof(RegisterDeliveryManAsync)}.");

            await Validate(request);

            var encryptedPassword = _passwordEncryptor.Encrypt(request.Password);

            var user = request.ToEntity(encryptedPassword);

            await _deliveryManWriteOnlyrepository.CreateAsync(user);

            output.Succeeded(new MessageResult("Cadastro realizado com sucesso"));

            _logger.Information($"Fim {nameof(RegisterDeliveryManAsync)}.");
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

    private async Task Validate(RegisterDeliveryMan request)
    {
        _logger.Information($"Início {nameof(Validate)}.");

        var userValidator = new RegisterValidator();
        var validationResult = userValidator.Validate(request);

        var emailTask = _deliveryManReadOnlyrepository.RecoverByEmailAsync(request.Email);
        var cnpjTask = _deliveryManReadOnlyrepository.RecoverByCNPJAsync(request.CNPJ);
        var cnhTask = _deliveryManReadOnlyrepository.RecoverByCNHAsync(request.CNHNumber);

        await Task.WhenAll(emailTask, cnpjTask, cnhTask);

        var validations = new (object Result, string Field, string Error)[]
        {
            (emailTask.Result, "email", ErrorsMessages.EmailAlreadyRegistered),
            (cnpjTask.Result, "cnpj", ErrorsMessages.CNPJAlreadyRegistered),
            (cnhTask.Result, "cnh", ErrorsMessages.CNHAlreadyRegistered)
        };

        validationResult.Errors.AddRange(
            validations
                .Where(x => x.Result is not null)
                .Select(x => new FluentValidation.Results.ValidationFailure(x.Field, x.Error))
        );

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorsException(errorMessages);
        }
    }
}