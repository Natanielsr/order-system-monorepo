#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.Addresses.Commands.DisableAddress;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Addresses.Commands.DisableAddress;

public class DisableAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DisableAddressHandler _handler;

    public DisableAddressHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DisableAddressHandler(_addressRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_DisableAddress_SuccessfullyReturnsAddressDto()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var disabledAddress = new Address
        {
            Id = addressId,
            FullName = "Test User",
            Cpf = "12345678901",
            Street = "Rua Test",
            Number = "123",
            Neighborhood = "Bairro",
            City = "Cidade",
            State = "SP",
            ZipCode = "12345678",
            Country = "Brasil",
            UserId = userId,
            IsDefault = false,
            Active = false, // Desativado
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var addressDto = new AddressDto
        {
            Id = addressId,
            FullName = "Test User",
            Street = "Rua Test",
            Number = "123",
            Neighborhood = "Bairro",
            City = "Cidade",
            State = "SP",
            ZipCode = "12345678",
            UserId = userId,
            IsDefault = false
        };

        _addressRepositoryMock
            .Setup(r => r.DisableAsync(addressId))
            .ReturnsAsync(disabledAddress);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<AddressDto>(disabledAddress))
            .Returns(addressDto);

        var command = new DisableAddressCommand(addressId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressId, result.Id);
        _addressRepositoryMock.Verify(r => r.DisableAsync(addressId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var expectedException = new Exception("Database error");

        _addressRepositoryMock
            .Setup(r => r.DisableAsync(addressId))
            .ThrowsAsync(expectedException);

        var command = new DisableAddressCommand(addressId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
