using BikeDeliveryMan.Application.UseCase.Register;
using BikeDeliveryMan.Communication.Request;
using BikeDeliveryMan.Domain.Entities;
using BikeDeliveryMan.Domain.Repositories.Contracts.User;
using BikeRent.Exception.ExceptionBase;
using Moq;
using Serilog;
using TokenService.Manager.Controller;

namespace BikeDeliveryMan.Test.UseCases;

public class RegisterUseCaseTests
{
    private readonly Mock<IDeliveryManReadOnly> _readOnlyRepoMock;
    private readonly Mock<IDeliveryManWriteOnly> _writeOnlyRepoMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly PasswordEncryptor _passwordEncryptor;
    private readonly RegisterUseCase _sut;

    public RegisterUseCaseTests()
    {
        _readOnlyRepoMock = new Mock<IDeliveryManReadOnly>();
        _writeOnlyRepoMock = new Mock<IDeliveryManWriteOnly>();
        _loggerMock = new Mock<ILogger>();
        _passwordEncryptor = new PasswordEncryptor("any-key");

        _sut = new RegisterUseCase(
            _readOnlyRepoMock.Object,
            _writeOnlyRepoMock.Object,
            _passwordEncryptor,
            _loggerMock.Object
        );
    }

    private RegisterDeliveryMan CreateRequest() =>
        new RegisterDeliveryMan(
            "Test User",
            "test@email.com",
            "123456",
            "74483191000126",
            DateTime.Now.AddYears(-30),
            "999999999",
            "AB");

    [Fact]
    public async Task RegisterDeliveryManAsync_ShouldSucceed_WhenValidRequest()
    {
        // Arrange
        var request = CreateRequest();

        _readOnlyRepoMock.Setup(r => r.RecoverByEmailAsync(request.Email))
            .ReturnsAsync((DeliveryMan)null);
        _readOnlyRepoMock.Setup(r => r.RecoverByCNPJAsync(request.CNPJ))
            .ReturnsAsync((DeliveryMan)null);
        _readOnlyRepoMock.Setup(r => r.RecoverByCNHAsync(request.CNHNumber))
            .ReturnsAsync((DeliveryMan)null);

        // Act
        var result = await _sut.RegisterDeliveryManAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Cadastro realizado com sucesso", result.Data.Message);
        _writeOnlyRepoMock.Verify(r => r.CreateAsync(It.IsAny<DeliveryMan>()), Times.Once);
    }

    [Fact]
    public async Task RegisterDeliveryManAsync_ShouldFail_WhenEmailAlreadyExists()
    {
        // Arrange
        var request = CreateRequest();
        _readOnlyRepoMock.Setup(r => r.RecoverByEmailAsync(request.Email))
            .ReturnsAsync(new DeliveryMan()); // simula existente

        // Act
        var result = await _sut.RegisterDeliveryManAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorsMessages.EmailAlreadyRegistered, result.Errors[0]);
        _writeOnlyRepoMock.Verify(r => r.CreateAsync(It.IsAny<DeliveryMan>()), Times.Never);
    }

    [Fact]
    public async Task RegisterDeliveryManAsync_ShouldFail_WhenCNPJAlreadyExists()
    {
        //Arrange
        var request = CreateRequest();
        _readOnlyRepoMock.Setup(r => r.RecoverByCNPJAsync(request.CNPJ))
            .ReturnsAsync(new DeliveryMan());

        //Act
        var result = await _sut.RegisterDeliveryManAsync(request);

        //Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorsMessages.CNPJAlreadyRegistered, result.Errors[0]);
    }

    [Fact]
    public async Task RegisterDeliveryManAsync_ShouldFail_WhenCNHAlreadyExists()
    {
        //Assert
        var request = CreateRequest();
        _readOnlyRepoMock.Setup(r => r.RecoverByCNHAsync(request.CNHNumber))
            .ReturnsAsync(new DeliveryMan());

        //Act
        var result = await _sut.RegisterDeliveryManAsync(request);

        //Assert
        Assert.False(result.Success);
        Assert.Equal(ErrorsMessages.CNHAlreadyRegistered, result.Errors[0]);
    }

    [Fact]
    public async Task RegisterDeliveryManAsync_ShouldFail_WhenUnexpectedErrorOccurs()
    {
        //Assert
        var request = CreateRequest();

        _readOnlyRepoMock.Setup(r => r.RecoverByEmailAsync(request.Email))
            .ReturnsAsync((DeliveryMan)null);
        _readOnlyRepoMock.Setup(r => r.RecoverByCNPJAsync(request.CNPJ))
            .ReturnsAsync((DeliveryMan)null);
        _readOnlyRepoMock.Setup(r => r.RecoverByCNHAsync(request.CNHNumber))
            .ReturnsAsync((DeliveryMan)null);

        _writeOnlyRepoMock.Setup(r => r.CreateAsync(It.IsAny<DeliveryMan>()))
            .ThrowsAsync(new Exception("Falha no banco"));

        //Act
        var result = await _sut.RegisterDeliveryManAsync(request);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Algo deu errado: Falha no banco", result.Errors[0]);
    }
}
