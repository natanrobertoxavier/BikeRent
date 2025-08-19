using BikeAdm.Application.UseCase.Register;
using BikeAdm.Communication.Request;
using BikeAdm.Domain.Repositories.Contracts.User;
using Moq;
using Serilog;
using TokenService.Manager.Controller;

namespace BikeAdm.Tests.UseCases;

public class RegisterUseCaseTests
{
    private readonly Mock<IUserReadOnly> _userReadOnlyMock = new();
    private readonly Mock<IUserWriteOnly> _userWriteOnlyMock = new();
    private readonly Mock<PasswordEncryptor> _passwordEncryptorMock = new("any-string");
    private readonly Mock<ILogger> _loggerMock = new();

    private RegisterUseCase CreateUseCase() =>
        new(_userReadOnlyMock.Object, _userWriteOnlyMock.Object, _passwordEncryptorMock.Object, _loggerMock.Object);

    [Fact]
    public async Task RegisterUserAsync_SuccessfulRegistration_ReturnsSuccessResult()
    {
        // Arrange
        var request = new RequestRegisterUser("Test", "test@email.com", "123456");
        _userReadOnlyMock.Setup(r => r.RecoverByEmailAsync(request.Email)).ReturnsAsync((Domain.Entities.User)null);
        _userWriteOnlyMock.Setup(w => w.CreateAsync(It.IsAny<BikeAdm.Domain.Entities.User>())).Returns(Task.CompletedTask);

        var useCase = CreateUseCase();

        // Act
        var result = await useCase.RegisterUserAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Cadastro realizado com sucesso", result.Data.Message);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public async Task RegisterUserAsync_EmailAlreadyRegistered_ReturnsValidationError()
    {
        // Arrange
        var request = new RequestRegisterUser("Test", "test@email.com", "123456");
        _userReadOnlyMock.Setup(r => r.RecoverByEmailAsync(request.Email)).ReturnsAsync(new Domain.Entities.User());
        var useCase = CreateUseCase();

        // Act
        var result = await useCase.RegisterUserAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Email já cadastrado", result.Errors[0]);
    }

    [Fact]
    public async Task RegisterUserAsync_UnexpectedException_ReturnsFailure()
    {
        // Arrange
        var request = new RequestRegisterUser("Test", "test@email.com", "123456");
        _userReadOnlyMock.Setup(r => r.RecoverByEmailAsync(request.Email)).ThrowsAsync(new Exception("DB error"));
        var useCase = CreateUseCase();

        // Act
        var result = await useCase.RegisterUserAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Errors);
        Assert.Contains("Algo deu errado", result.Errors[0]);
    }
}
