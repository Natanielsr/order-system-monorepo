#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.Addresses.Queries.GetUserAddresses;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Addresses.Queries.GetUserAddresses;

public class GetUserAddressesHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserAddressesHandler _handler;

    public GetUserAddressesHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserAddressesHandler(_addressRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserHasAddresses_ReturnsAddressDtos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var addresses = new List<Address>
        {
            new Address
            {
                Id = Guid.NewGuid(),
                FullName = "User 1",
                Cpf = "12345678901",
                Street = "Rua 1",
                Number = "100",
                Neighborhood = "Bairro 1",
                City = "Cidade 1",
                State = "SP",
                ZipCode = "12345678",
                Country = "Brasil",
                UserId = userId,
                IsDefault = true,
                Active = true,
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow
            },
            new Address
            {
                Id = Guid.NewGuid(),
                FullName = "User 2",
                Cpf = "98765432101",
                Street = "Rua 2",
                Number = "200",
                Neighborhood = "Bairro 2",
                City = "Cidade 2",
                State = "RJ",
                ZipCode = "98765432",
                Country = "Brasil",
                UserId = userId,
                IsDefault = false,
                Active = true,
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow
            }
        };

        var addressesDto = addresses.Select(a => new AddressDto
        {
            Id = a.Id,
            FullName = a.FullName,
            Street = a.Street,
            Number = a.Number,
            Neighborhood = a.Neighborhood,
            City = a.City,
            State = a.State,
            ZipCode = a.ZipCode,
            UserId = a.UserId,
            IsDefault = a.IsDefault
        }).ToList();

        _addressRepositoryMock
            .Setup(r => r.GetUserAddressesAsync(userId))
            .ReturnsAsync(addresses);

        _mapperMock
            .Setup(m => m.Map<List<AddressDto>>(addresses))
            .Returns(addressesDto);

        var query = new GetUserAddressesQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Equal(userId, a.UserId));
        _addressRepositoryMock.Verify(r => r.GetUserAddressesAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoAddresses_ReturnsEmptyList()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _addressRepositoryMock
            .Setup(r => r.GetUserAddressesAsync(userId))
            .ReturnsAsync(new List<Address>());

        _mapperMock
            .Setup(m => m.Map<List<AddressDto>>(It.IsAny<List<Address>>()))
            .Returns(new List<AddressDto>());

        var query = new GetUserAddressesQuery(userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedException = new Exception("Database error");

        _addressRepositoryMock
            .Setup(r => r.GetUserAddressesAsync(userId))
            .ThrowsAsync(expectedException);

        var query = new GetUserAddressesQuery(userId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }
}
