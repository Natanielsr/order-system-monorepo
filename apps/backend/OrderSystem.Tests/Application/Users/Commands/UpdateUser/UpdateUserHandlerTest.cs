using AutoMapper;
using MediatR;
using Moq;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Users.Commands.UpdateUser;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSystem.Tests.Application.Users.Commands.UpdateUser;

public class UpdateUserHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateUserHandler _handler;

    private readonly Guid _userId;

    public UpdateUserHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userId = Guid.NewGuid();

        _handler = new UpdateUserHandler(
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingUser_UpdatesAndReturnsUserDto()
    {
        // Arrange
        var existingUser = User.CreateUser(
            id: _userId,
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );

        var command = new UpdateUserCommand(
            id: _userId,
            Phone: "2222222222"
        );

        var updatedUser = User.CreateUser(
            id: _userId,
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "2222222222"
        );

        var userDto = new UserDto
        {
            Id = _userId,
            Username = "testuser",
            Email = "test@example.com",
            Phone = "2222222222",
            Role = "User"
        };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(_userId))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(_userId, existingUser))
            .ReturnsAsync(updatedUser);
        _unitOfWorkMock.Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(updatedUser))
            .Returns(userDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_userId, result.Id);
        Assert.Equal("2222222222", result.Phone);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(_userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(_userId, It.Is<User>(u => u.Phone == "2222222222")), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(updatedUser), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingUser_ReturnsNull()
    {
        // Arrange
        var command = new UpdateUserCommand(
            id: _userId,
            Phone: "2222222222"
        );

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(_userId))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(_userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Entity>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCommitFails_ReturnsNull()
    {
        // Arrange
        var existingUser = User.CreateUser(
            id: _userId,
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );

        var command = new UpdateUserCommand(
            id: _userId,
            Phone: "2222222222"
        );

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(_userId))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(_userId, existingUser))
            .ReturnsAsync(existingUser);
        _unitOfWorkMock.Setup(uow => uow.CommitAsync())
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(_userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(_userId, existingUser), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var command = new UpdateUserCommand(
            id: _userId,
            Phone: "2222222222"
        );

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(_userId))
            .ThrowsAsync(new Exception("Database connection error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Assert.Equal("Database connection error", exception.Message);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(_userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Entity>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Never);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdatesUserPhoneAndRenewUpdateDate()
    {
        // Arrange
        var existingUser = User.CreateUser(
            id: _userId,
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1111111111"
        );

        var originalUpdateDate = existingUser.UpdateDate;

        var command = new UpdateUserCommand(
            id: _userId,
            Phone: "3333333333"
        );

        User capturedUser = null!;

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(_userId))
            .ReturnsAsync(existingUser);
        _userRepositoryMock.Setup(repo => repo.UpdateAsync(_userId, It.IsAny<Entity>()))
            .Callback<Guid, Entity>((id, entity) => capturedUser = (User)entity)
            .ReturnsAsync((Guid id, Entity entity) => (User)entity);
        _unitOfWorkMock.Setup(uow => uow.CommitAsync())
            .ReturnsAsync(true);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(It.IsAny<User>()))
            .Returns((User u) => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("3333333333", capturedUser.Phone);
        Assert.True(capturedUser.UpdateDate > originalUpdateDate);

        _userRepositoryMock.Verify(repo => repo.UpdateAsync(_userId, It.Is<User>(u => u.Phone == "3333333333" && u.UpdateDate > originalUpdateDate)), Times.Once);
    }
}
