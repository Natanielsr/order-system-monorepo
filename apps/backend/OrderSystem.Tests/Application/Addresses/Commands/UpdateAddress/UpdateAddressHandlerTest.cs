#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.Addresses.Commands.UpdateAddress;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Addresses.Commands.UpdateAddress;

public class UpdateAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateAddressHandler _handler;

    public UpdateAddressHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateAddressHandler(_addressRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_UpdateAddress_SuccessfullyReturnsAddressDto()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new UpdateAddressCommand(
            addressId,
            "Updated Name",
            "12345678901",
            "Updated Street",
            "456",
            "Apto 10",
            "Updated Neighborhood",
            "Updated City",
            "SP",
            "12345678",
            "Brasil",
            userId,
            false
        );

        var existingAddress = new Address
        {
            Id = addressId,
            FullName = "Old Name",
            Cpf = "98765432101",
            Street = "Old Street",
            Number = "123",
            Neighborhood = "Old Neighborhood",
            City = "Old City",
            State = "RJ",
            ZipCode = "98765432",
            Country = "Brasil",
            UserId = userId,
            IsDefault = false,
            Active = true,
            CreationDate = DateTimeOffset.UtcNow.AddDays(-10),
            UpdateDate = DateTimeOffset.UtcNow.AddDays(-5)
        };

        var updatedAddress = new Address
        {
            Id = addressId,
            FullName = "Updated Name",
            Cpf = "12345678901",
            Street = "Updated Street",
            Number = "456",
            Complement = "Apto 10",
            Neighborhood = "Updated Neighborhood",
            City = "Updated City",
            State = "SP",
            ZipCode = "12345678",
            Country = "Brasil",
            UserId = userId,
            IsDefault = false,
            Active = true,
            CreationDate = existingAddress.CreationDate,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var addressDto = new AddressDto
        {
            Id = addressId,
            FullName = "Updated Name",
            Street = "Updated Street",
            Number = "456",
            Complement = "Apto 10",
            Neighborhood = "Updated Neighborhood",
            City = "Updated City",
            State = "SP",
            ZipCode = "12345678",
            UserId = userId,
            IsDefault = false
        };

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ReturnsAsync(existingAddress);

        _mapperMock
            .Setup(m => m.Map<Address>(command))
            .Returns(updatedAddress);

        _addressRepositoryMock
            .Setup(r => r.UpdateAsync(addressId, updatedAddress))
            .ReturnsAsync(updatedAddress);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<AddressDto>(updatedAddress))
            .Returns(addressDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addressId, result.Id);
        Assert.Equal("Updated Name", result.FullName);
        Assert.Equal("Updated Street", result.Street);
        _addressRepositoryMock.Verify(r => r.UpdateAsync(addressId, updatedAddress), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_SetAsDefault_UnsetsOtherAddresses()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new UpdateAddressCommand(
            addressId,
            "Main Address",
            "12345678901",
            "Main Street",
            "100",
            null,
            "Main Neighborhood",
            "Main City",
            "SP",
            "12345678",
            "Brasil",
            userId,
            true
        );

        var existingAddress = new Address
        {
            Id = addressId,
            FullName = "Old Name",
            Cpf = "98765432101",
            Street = "Old Street",
            Number = "123",
            Neighborhood = "Old Neighborhood",
            City = "Old City",
            State = "SP",
            ZipCode = "12345678",
            Country = "Brasil",
            UserId = userId,
            IsDefault = false,
            Active = true,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var otherAddress = new Address
        {
            Id = Guid.NewGuid(),
            FullName = "Other Address",
            Cpf = "11122233344",
            Street = "Other Street",
            Number = "200",
            Neighborhood = "Other Neighborhood",
            City = "Other City",
            State = "SP",
            ZipCode = "87654321",
            Country = "Brasil",
            UserId = userId,
            IsDefault = true,
            Active = true,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var allUserAddresses = new List<Address> { existingAddress, otherAddress };

        var updatedAddress = new Address
        {
            Id = addressId,
            FullName = "Main Address",
            Cpf = "12345678901",
            Street = "Main Street",
            Number = "100",
            Complement = null,
            Neighborhood = "Main Neighborhood",
            City = "Main City",
            State = "SP",
            ZipCode = "12345678",
            Country = "Brasil",
            UserId = userId,
            IsDefault = true,
            Active = true,
            CreationDate = existingAddress.CreationDate,
            UpdateDate = DateTimeOffset.UtcNow
        };

        var addressDto = new AddressDto
        {
            Id = addressId,
            FullName = "Main Address",
            Street = "Main Street",
            Number = "100",
            Neighborhood = "Main Neighborhood",
            City = "Main City",
            State = "SP",
            ZipCode = "12345678",
            UserId = userId,
            IsDefault = true
        };

        _addressRepositoryMock
            .Setup(r => r.GetUserAddressesAsync(userId))
            .ReturnsAsync(allUserAddresses);

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ReturnsAsync(existingAddress);

        _mapperMock
            .Setup(m => m.Map<Address>(command))
            .Returns(updatedAddress);

        // O repository.UpdateAsync deve atualizar o endereço
        _addressRepositoryMock
            .Setup(r => r.UpdateAsync(addressId, It.IsAny<Address>()))
            .ReturnsAsync((Guid id, Address addr) => addr);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(true);

        _mapperMock
            .Setup(m => m.Map<AddressDto>(updatedAddress))
            .Returns(addressDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsDefault);
        // Verifica que o outro endereço foi desmarcado como default
        var otherAddressAfter = allUserAddresses.First(a => a.Id == otherAddress.Id);
        Assert.False(otherAddressAfter.IsDefault);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CommitFails_ThrowsException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new UpdateAddressCommand(
            addressId,
            "Test",
            "12345678901",
            "Rua Test",
            "123",
            null,
            "Bairro",
            "Cidade",
            "SP",
            "12345678",
            "Brasil",
            userId,
            false
        );

        var existingAddress = new Address
        {
            Id = addressId,
            FullName = "Test",
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
            Active = true,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow
        };

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ReturnsAsync(existingAddress);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("It was not possible to update the address in the repository.", exception.Message);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var command = new UpdateAddressCommand(
            addressId,
            "Test",
            "12345678901",
            "Rua Test",
            "123",
            null,
            "Bairro",
            "Cidade",
            "SP",
            "12345678",
            "Brasil",
            userId,
            false
        );

        var expectedException = new Exception("Database update error");

        _addressRepositoryMock
            .Setup(r => r.GetByIdAsync(addressId))
            .ThrowsAsync(expectedException);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
