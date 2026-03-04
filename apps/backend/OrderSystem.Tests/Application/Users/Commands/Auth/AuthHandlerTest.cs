

#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Users.Commands.Auth;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.Services;
using OrderSystem.Application.Services;

namespace OrderSystem.Tests.Application.Users.Commands.Auth;

public class AuthHandlerTest
{
    private readonly AuthHandler _authHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordService> _mockPasswordService;
    private readonly Mock<ITokenService> _mockTokenService;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly User _testUser;

    public AuthHandlerTest()
    {
        // Configurar mocks
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordService = new Mock<IPasswordService>();
        _mockTokenService = new Mock<ITokenService>();

        // Criar usuário de teste
        _testUser = User.CreateUser(
            _userId,
            "testuser",
            "test@email.com",
            "hashedpassword123",
            UserRole.User,
            "11999998888"
        );

        // Setup do repositório
        _mockUserRepository.Setup(u => u.GetByUserNameAsync("testuser"))
            .ReturnsAsync(_testUser);
        _mockUserRepository.Setup(u => u.GetByUserNameAsync("wronguser"))
            .ReturnsAsync((User)null!);

        // Setup do password service
        _mockPasswordService.Setup(p => p.VerifyPassowrd("correctpass", "hashedpassword123"))
            .Returns(true);
        _mockPasswordService.Setup(p => p.VerifyPassowrd("wrongpass", "hashedpassword123"))
            .Returns(false);

        // Setup do token service
        _mockTokenService.Setup(t => t.GenerateToken(_testUser))
            .Returns("jwt-token-123");

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
            cfg.AddProfile<UserMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _authHandler = new AuthHandler(
            _mockUserRepository.Object,
            _mockPasswordService.Object,
            _mapper,
            _mockTokenService.Object
        );
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthResponseDto()
    {
        // Arrange
        var command = new AuthCommand("testuser", "correctpass");

        // Act
        var result = await _authHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testUser.Username, result.Username);
        Assert.Equal(_testUser.Email, result.Email);
        Assert.Equal("jwt-token-123", result.Token);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsUserNotFoundException()
    {
        // Arrange
        var command = new AuthCommand("unknownuser", "anypass");

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _authHandler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsInvalidPasswordException()
    {
        // Arrange
        var command = new AuthCommand("testuser", "wrongpass");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPasswordException>(
            () => _authHandler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_PasswordServiceThrowsException_Propagates()
    {
        // Arrange
        _mockPasswordService.Setup(p => p.VerifyPassowrd(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new Exception("Password service error"));

        var command = new AuthCommand("testuser", "anypass");

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _authHandler.Handle(command, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_UserRepositoryThrowsException_Propagates()
    {
        // Arrange
        _mockUserRepository.Setup(u => u.GetByUserNameAsync("testuser"))
            .ThrowsAsync(new Exception("Database error"));

        var command = new AuthCommand("testuser", "anypass");

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _authHandler.Handle(command, CancellationToken.None)
        );
    }
}