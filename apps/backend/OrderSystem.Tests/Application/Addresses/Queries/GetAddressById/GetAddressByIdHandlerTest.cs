#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.Addresses.Queries.GetAddressById;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Addresses.Queries.GetAddressById;

public class GetAddressByIdHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAddressByIdHandler _handler;

    public GetAddressByIdHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAddressByIdHandler(_addressRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingAddress_ReturnsAddressDto()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var address = new Address
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
            UserId = Guid.NewGuid(),
            IsDefault = false,
            Active = true,
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
            UserId = address.UserId,
            IsDefault = false
        };

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ReturnsAsync(address);

        _mapperMock
            .Setup(m => m.Map<AddressDto>(address))
            .Returns(addressDto);

        var query = new GetAddressByIdQuery(addressId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressId, result.Id);
        Assert.Equal("Test User", result.FullName);
        _addressRepositoryMock.Verify(r => r.GetByIdAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAddress_ReturnsNull()
    {
        // Arrange
        var addressId = Guid.NewGuid();

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ReturnsAsync((Address)null);

        var query = new GetAddressByIdQuery(addressId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var expectedException = new Exception("Database error");

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ThrowsAsync(expectedException);

        var query = new GetAddressByIdQuery(addressId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }
}
