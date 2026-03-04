#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Address;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Addresses.Commands.CreateAddress;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Addresses.Commands.CreateAddress;

public class CreateAddressHandlerTest
{
    private readonly CreateAddressHandler _createAddressHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IAddressRepository> _mockAddressRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Address _createdAddress;

    public CreateAddressHandlerTest()
    {
        // Configurar mocks
        _mockAddressRepository = new Mock<IAddressRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Criar endereço de teste
        _createdAddress = new Address()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            FullName = "João Silva",
            Cpf = "123.456.789-00",
            Street = "Rua das Flores",
            Number = "123",
            Complement = "Apto 45",
            Neighborhood = "Centro",
            City = "São Paulo",
            State = "SP",
            ZipCode = "01234-567",
            Country = "Brasil",
            UserId = _userId,
            IsDefault = true
        };

        // Setup do repositório - GetUserAddressesAsync
        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ReturnsAsync(new List<Address>());

        // Setup do repository.AddAsync
        _mockAddressRepository.Setup(r => r.AddAsync(It.IsAny<Address>()))
            .ReturnsAsync(_createdAddress);

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AddressMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _createAddressHandler = new CreateAddressHandler(
            _mockAddressRepository.Object,
            _mapper,
            _mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesAddressAndReturnsResponse()
    {
        // Arrange
        var command = new CreateAddressCommand(
            FullName: "João Silva",
            Cpf: "123.456.789-00",
            Street: "Rua das Flores",
            Number: "123",
            Complement: "Apto 45",
            Neighborhood: "Centro",
            City: "São Paulo",
            State: "SP",
            ZipCode: "01234-567",
            UserId: _userId,
            IsDefault: true
        );

        // Act
        var result = await _createAddressHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_createdAddress.Id, result.Id);
        Assert.Equal("João Silva", result.FullName);
        Assert.Equal("123.456.789-00", result.Cpf);
        Assert.Equal("Rua das Flores", result.Street);
        Assert.Equal("123", result.Number);
        Assert.Equal("Apto 45", result.Complement);
        Assert.Equal("Centro", result.Neighborhood);
        Assert.Equal("São Paulo", result.City);
        Assert.Equal("SP", result.State);
        Assert.Equal("01234-567", result.ZipCode);
        Assert.Equal("Brasil", result.Country);
        Assert.Equal(_userId, result.UserId);
        Assert.True(result.IsDefault);

        // Verify
        _mockAddressRepository.Verify(r => r.GetUserAddressesAsync(_userId), Times.Once);
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_UserHasTooManyAddresses_ThrowsAddressCountExceededException()
    {
        // Arrange - usuário já tem 5 endereços
        var existingAddresses = new List<Address>()
        {
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 1",
                Cpf = "111.111.111-11",
                Street = "Rua 1",
                Number = "1",
                Neighborhood = "Bairro 1",
                City = "Cidade 1",
                State = "SP",
                ZipCode = "00000-001",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            },
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 2",
                Cpf = "222.222.222-22",
                Street = "Rua 2",
                Number = "2",
                Neighborhood = "Bairro 2",
                City = "Cidade 2",
                State = "RJ",
                ZipCode = "00000-002",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            },
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 3",
                Cpf = "333.333.333-33",
                Street = "Rua 3",
                Number = "3",
                Neighborhood = "Bairro 3",
                City = "Cidade 3",
                State = "MG",
                ZipCode = "00000-003",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            },
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 4",
                Cpf = "444.444.444-44",
                Street = "Rua 4",
                Number = "4",
                Neighborhood = "Bairro 4",
                City = "Cidade 4",
                State = "ES",
                ZipCode = "00000-004",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            },
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 5",
                Cpf = "555.555.555-55",
                Street = "Rua 5",
                Number = "5",
                Neighborhood = "Bairro 5",
                City = "Cidade 5",
                State = "BA",
                ZipCode = "00000-005",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            },
            new Address
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                FullName = "User 6",
                Cpf = "666.666.666-66",
                Street = "Rua 6",
                Number = "6",
                Neighborhood = "Bairro 6",
                City = "Cidade 6",
                State = "RJ",
                ZipCode = "00000-006",
                Country = "Brasil",
                UserId = _userId,
                IsDefault = false
            }
        };

        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ReturnsAsync(existingAddresses);

        var command = new CreateAddressCommand(
            "Novo Endereço",
            "987.654.321-00",
            "Rua Nova",
            "456",
            "",
            "Bairro Novo",
            "Rio de Janeiro",
            "RJ",
            "20000-000",
            _userId,
            false
        );

        // Act & Assert
        await Assert.ThrowsAsync<AddressCountExceededException>(
            () => _createAddressHandler.Handle(command, CancellationToken.None)
        );

        // Verify: AddAsync não foi chamado
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_IsDefaultTrue_DisablesOtherAddresses()
    {
        // Arrange - usuário tem 2 endereços existentes
        var existingAddress1 = new Address()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            FullName = "Endereço 1",
            Cpf = "111.111.111-11",
            Street = "Rua 1",
            Number = "1",
            Neighborhood = "Bairro 1",
            City = "Cidade 1",
            State = "SP",
            ZipCode = "00000-001",
            Country = "Brasil",
            UserId = _userId,
            IsDefault = true
        };
        var existingAddress2 = new Address()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            FullName = "Endereço 2",
            Cpf = "222.222.222-22",
            Street = "Rua 2",
            Number = "2",
            Neighborhood = "Bairro 2",
            City = "Cidade 2",
            State = "RJ",
            ZipCode = "00000-002",
            Country = "Brasil",
            UserId = _userId,
            IsDefault = false
        };

        var existingAddresses = new List<Address>() { existingAddress1, existingAddress2 };

        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ReturnsAsync(existingAddresses);

        var command = new CreateAddressCommand(
            "Novo Endereço Default",
            "987.654.321-00",
            "Rua Nova",
            "456",
            "",
            "Bairro Novo",
            "Rio de Janeiro",
            "RJ",
            "20000-000",
            _userId,
            IsDefault: true  // Novo endereço será default
        );

        // Act
        var result = await _createAddressHandler.Handle(command, CancellationToken.None);

        // Assert - Verificar que os outros endereços foram modificados
        Assert.True(existingAddress1.IsDefault == false);
        Assert.True(existingAddress2.IsDefault == false);
        Assert.True(result.IsDefault);

        // Verify - GetUserAddresses e Add foram chamados
        _mockAddressRepository.Verify(r => r.GetUserAddressesAsync(_userId), Times.Once);
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_IsDefaultFalse_DoesNotDisableOtherAddresses()
    {
        // Arrange - usuário tem 2 endereços existentes
        var existingAddress1 = new Address()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            FullName = "Endereço 1",
            Cpf = "111.111.111-11",
            Street = "Rua 1",
            Number = "1",
            Neighborhood = "Bairro 1",
            City = "Cidade 1",
            State = "SP",
            ZipCode = "00000-001",
            Country = "Brasil",
            UserId = _userId,
            IsDefault = true
        };
        var existingAddress2 = new Address()
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            FullName = "Endereço 2",
            Cpf = "222.222.222-22",
            Street = "Rua 2",
            Number = "2",
            Neighborhood = "Bairro 2",
            City = "Cidade 2",
            State = "RJ",
            ZipCode = "00000-002",
            Country = "Brasil",
            UserId = _userId,
            IsDefault = false
        };

        var existingAddresses = new List<Address>() { existingAddress1, existingAddress2 };

        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ReturnsAsync(existingAddresses);

        var command = new CreateAddressCommand(
            "Novo Endereço Normal",
            "987.654.321-00",
            "Rua Nova",
            "456",
            "",
            "Bairro Novo",
            "Rio de Janeiro",
            "RJ",
            "20000-000",
            _userId,
            IsDefault: false  // Novo endereço NÃO é default
        );

        // Act
        var result = await _createAddressHandler.Handle(command, CancellationToken.None);

        // Assert - Verificar que os outros endereços NÃO foram modificados
        Assert.True(existingAddress1.IsDefault); // ainda é default
        Assert.False(existingAddress2.IsDefault);

        // Verify - apenas GetUserAddresses e Add, sem modificar outros
        _mockAddressRepository.Verify(r => r.GetUserAddressesAsync(_userId), Times.Once);
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_Propagates()
    {
        // Arrange
        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ThrowsAsync(new Exception("Database error"));

        var command = new CreateAddressCommand(
            "Endereço",
            "123.456.789-00",
            "Rua Teste",
            "123",
            "",
            "Centro",
            "São Paulo",
            "SP",
            "01234-567",
            _userId,
            false
        );

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _createAddressHandler.Handle(command, CancellationToken.None)
        );

        // Verify: AddAsync e Commit não chamados
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyUserAddresses_AllowsCreation()
    {
        // Arrange - usuário não tem endereços
        _mockAddressRepository.Setup(r => r.GetUserAddressesAsync(_userId))
            .ReturnsAsync(new List<Address>());

        var command = new CreateAddressCommand(
            "Primeiro Endereço",
            "123.456.789-00",
            "Rua Primeira",
            "1",
            "",
            "Centro",
            "São Paulo",
            "SP",
            "01234-567",
            _userId,
            IsDefault: true
        );

        // Act
        var result = await _createAddressHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsDefault);

        // Verify
        _mockAddressRepository.Verify(r => r.GetUserAddressesAsync(_userId), Times.Once);
        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }
}
