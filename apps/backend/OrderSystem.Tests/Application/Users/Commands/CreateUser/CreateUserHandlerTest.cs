#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.User;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Users.Commands.CreateUser;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.Services;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Users.Commands.CreateUser;

public class CreateUserHandlerTest
{
    private readonly CreateUserHandler _createUserHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordService> _mockPasswordService;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly string _username = "newuser";
    private readonly string _email = "newuser@email.com";
    private readonly string _password = "Password123!";

    public CreateUserHandlerTest()
    {
        // Configurar mocks
        _mockUserRepository = new Mock<IUserRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordService = new Mock<IPasswordService>();

        // Setup do password service
        _mockPasswordService.Setup(p => p.HashPassword(_password))
            .Returns("hashed_password_123");

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _createUserHandler = new CreateUserHandler(
            _mockUserRepository.Object,
            _mockUnitOfWork.Object,
            _mapper,
            _mockPasswordService.Object
        );
    }

    [Fact]
    public async Task Handle_ValidUser_CreatesUserAndReturnsResponse()
    {
        // Arrange
        var command = new CreateUserCommand(_username, _email, _password, _password);

        // Garantir que username e email não existem
        _mockUserRepository.Setup(u => u.GetByUserNameAsync(_username))
            .ReturnsAsync((User)null);
        _mockUserRepository.Setup(u => u.GetByEmailAsync(_email))
            .ReturnsAsync((User)null);

        Entity createdEntity = null;
        _mockUserRepository.Setup(u => u.AddAsync(It.IsAny<Entity>()))
            .Callback<Entity>(e => createdEntity = e)
            .ReturnsAsync((Entity e) => e);

        // Act
        var result = await _createUserHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_username, result.Username);
        Assert.Equal(_email, result.Email);
        Assert.NotNull(createdEntity);
        var createdUser = (User)createdEntity;
        Assert.Equal(_username, createdUser.Username);
        Assert.Equal(_email, createdUser.Email);
        Assert.Equal("hashed_password_123", createdUser.HashedPassword);
        Assert.Equal(UserRole.User, createdUser.Role);

        // Verify
        _mockUserRepository.Verify(u => u.GetByUserNameAsync(_username), Times.Once);
        _mockUserRepository.Verify(u => u.GetByEmailAsync(_email), Times.Once);
        _mockUserRepository.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
        _mockPasswordService.Verify(p => p.HashPassword(_password), Times.Once);
    }

    [Fact]
    public async Task Handle_UsernameAlreadyExists_ThrowsUsernameAlreadyExistsException()
    {
        // Arrange
        var command = new CreateUserCommand(_username, _email, _password, _password);

        var existingUser = User.CreateUser(
            Guid.NewGuid(),
            _username,
            "other@email.com",
            "hashed",
            UserRole.User,
            "12345678900"
        );

        _mockUserRepository.Setup(u => u.GetByUserNameAsync(_username))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<UsernameAlreadyExistsException>(
            () => _createUserHandler.Handle(command, CancellationToken.None)
        );

        // Verify: GetByUserName foi chamado, mas AddAsync não
        _mockUserRepository.Verify(u => u.GetByUserNameAsync(_username), Times.Once);
        _mockUserRepository.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_EmailAlreadyExists_ThrowsEmailAlreadyExistsException()
    {
        // Arrange
        var command = new CreateUserCommand(_username, _email, _password, _password);

        var existingUser = User.CreateUser(
            Guid.NewGuid(),
            "otheruser",
            _email,
            "hashed",
            UserRole.User,
            "12345678900"
        );

        _mockUserRepository.Setup(u => u.GetByUserNameAsync(_username))
            .ReturnsAsync((User)null);
        _mockUserRepository.Setup(u => u.GetByEmailAsync(_email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<EmailAlreadyExistsException>(
            () => _createUserHandler.Handle(command, CancellationToken.None)
        );

        // Verify
        _mockUserRepository.Verify(u => u.GetByUserNameAsync(_username), Times.Once);
        _mockUserRepository.Verify(u => u.GetByEmailAsync(_email), Times.Once);
        _mockUserRepository.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_PasswordServiceThrowsException_Propagates()
    {
        // Arrange
        var command = new CreateUserCommand(_username, _email, _password, _password);

        _mockUserRepository.Setup(u => u.GetByUserNameAsync(_username))
            .ReturnsAsync((User)null);
        _mockUserRepository.Setup(u => u.GetByEmailAsync(_email))
            .ReturnsAsync((User)null);

        _mockPasswordService.Setup(p => p.HashPassword(It.IsAny<string>()))
            .Throws(new Exception("Hashing error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _createUserHandler.Handle(command, CancellationToken.None)
        );

        // Verify: AddAsync e Commit não chamados
        _mockUserRepository.Verify(u => u.AddAsync(It.IsAny<User>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_UserRepositoryAddThrowsException_Propagates()
    {
        // Arrange
        var command = new CreateUserCommand(_username, _email, _password, _password);

        _mockUserRepository.Setup(u => u.GetByUserNameAsync(_username))
            .ReturnsAsync((User)null);
        _mockUserRepository.Setup(u => u.GetByEmailAsync(_email))
            .ReturnsAsync((User)null);

        _mockUserRepository.Setup(u => u.AddAsync(It.IsAny<User>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _createUserHandler.Handle(command, CancellationToken.None)
        );

        // Verify: Commit não chamado
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }
}
