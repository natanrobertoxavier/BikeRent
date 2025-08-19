using BikeDeliveryMan.Application.Services;
using BikeDeliveryMan.Communication.Response;
using BikeDeliveryMan.Domain.Services;
using BikeRent.Exception.ExceptionBase;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace BikeDeliveryMan.Application.UseCase.SendPhoto;

public class SendPhotoUseCase(
    IPhotoStorageService photoStorageService,
    ILoggedUser loggedUser,
    ILogger logger) : ISendPhotoUseCase
{
    private readonly IPhotoStorageService _photoStorageService = photoStorageService;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ILogger _logger = logger;

    public async Task<Result<MessageResult>> SendPhotoAsync(IFormFile file)
    {
        var output = new Result<MessageResult>();

        try
        {
            _logger.Information($"Início {nameof(SendPhotoAsync)}.");

            var extension = ValidateAndGetExtension(file);
            var fileName = await GetFileName();
            fileName += extension;

            await _photoStorageService.SavePhotoAsync(fileName, file);

            output.Succeeded(new MessageResult("Foto persistida com sucesso"));
            _logger.Information($"Fim {nameof(SendPhotoAsync)}.");
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

    private static string ValidateAndGetExtension(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ValidationErrorsException(new() { ErrorsMessages.BlankPhoto });

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension != ".png" && extension != ".bmp")
            throw new ValidationErrorsException(new() { ErrorsMessages.FormatPhoto });

        return extension;
    }

    private async Task<string> GetFileName()
    {
        var loggedUser = await _loggedUser.GetLoggedUserAsync() ?? throw new BikeRentException("Usuário não autenticado");
        return loggedUser.Id.ToString();
    }
}
