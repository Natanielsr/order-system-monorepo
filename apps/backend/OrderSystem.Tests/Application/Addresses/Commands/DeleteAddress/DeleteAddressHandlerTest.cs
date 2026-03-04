#nullable disable

using Moq;
using OrderSystem.Application.Addresses.Commands.DeleteAddress;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Addresses.Commands.DeleteAddress;

public class DeleteAddressHandlerTest
{
    private readonly Mock<IAddressRepository> _addressRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteAddressHandler _handler;

    public DeleteAddressHandlerTest()
    {
        _addressRepositoryMock = new Mock<IAddressRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteAddressHandler(_addressRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_DeleteAddress_SuccessfullyReturnsTrue()
    {
        // Arrange
        var addressId = Guid.NewGuid();

        _addressRepositoryMock
            .Setup(r => r.DeleteAsync(addressId))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(true);

        var command = new DeleteAddressCommand(addressId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _addressRepositoryMock.Verify(r => r.DeleteAsync(addressId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_DeleteFails_ReturnsFalse()
    {
        // Arrange
        var addressId = Guid.NewGuid();

        _addressRepositoryMock
            .Setup(r => r.DeleteAsync(addressId))
            .ReturnsAsync(false);

        var command = new DeleteAddressCommand(addressId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _addressRepositoryMock.Verify(r => r.DeleteAsync(addressId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_CommitFails_ReturnsFalse()
    {
        // Arrange
        var addressId = Guid.NewGuid();

        _addressRepositoryMock
            .Setup(r => r.DeleteAsync(addressId))
            .ReturnsAsync(true);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ReturnsAsync(false);

        var command = new DeleteAddressCommand(addressId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _addressRepositoryMock.Verify(r => r.DeleteAsync(addressId), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var expectedException = new Exception("Database error");

        _addressRepositoryMock
            .Setup(r => r.DeleteAsync(addressId))
            .ThrowsAsync(expectedException);

        var command = new DeleteAddressCommand(addressId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
