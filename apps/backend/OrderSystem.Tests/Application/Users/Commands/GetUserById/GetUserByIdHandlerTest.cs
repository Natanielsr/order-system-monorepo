using AutoMapper;
using MediatR;
using Moq;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Users.Commands.GetUser;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;
using OrderSystem.Tests.Application.Users.Commands.GetUserById;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSystem.Tests.Application.Users.Commands.GetUserById;

public class GetUserByIdHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetUserByIdHandler _handler;

    public GetUserByIdHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetUserByIdHandler(_userRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingUser_ReturnsUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.CreateUser(
            id: userId,
            username: "testuser",
            email: "test@example.com",
            hashedPassword: "hashed123",
            role: "User",
            phone: "1234567890"
        );

        var userDto = new UserDto
        {
            Id = userId,
            Username = "testuser",
            Email = "test@example.com",
            Phone = "1234567890",
            Role = "User"
        };

        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<UserDto>(user))
                .Returns(userDto);

        var command = new GetUserByIdCommand(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("testuser", result.Username);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("1234567890", result.Phone);
        Assert.Equal("User", result.Role);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(user), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingUser_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User)null);

        var command = new GetUserByIdCommand(userId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId))
            .ThrowsAsync(new Exception("Database error"));

        var command = new GetUserByIdCommand(userId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, CancellationToken.None)
        );

        Assert.Equal("Database error", exception.Message);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<UserDto>(It.IsAny<User>()), Times.Never);
    }
}