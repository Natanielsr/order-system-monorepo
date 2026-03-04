#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Products.Queries.GetById;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Products.Queries.GetById;

public class GetProductByIdHandlerTest
{
    private readonly GetProductByIdHandler _getProductByIdHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IProductRepository> _mockProductRepository;

    private readonly Product _testProduct;
    private readonly Guid _productId;

    public GetProductByIdHandlerTest()
    {
        // Configurar mocks
        _mockProductRepository = new Mock<IProductRepository>();

        // Criar dados de teste
        _productId = Guid.NewGuid();
        _testProduct = new Product()
        {
            Id = _productId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            Name = "Test Product",
            Price = 25.99m,
            AvailableQuantity = 100,
            ImagePath = "/images/products/test.jpg"
        };

        _mockProductRepository.Setup(p => p.GetByIdAsync(_productId))
            .ReturnsAsync(_testProduct);

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _getProductByIdHandler = new GetProductByIdHandler(
            _mockProductRepository.Object,
            _mapper
        );
    }

    [Fact]
    public async Task Handle_ExistingProduct_ReturnsProductDto()
    {
        // Arrange
        var query = new GetProductByIdQuery(_productId);

        // Act
        var result = await _getProductByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_productId, result.Id);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal(25.99m, result.Price);
        Assert.Equal(100, result.AvailableQuantity);
        Assert.Equal("/images/products/test.jpg", result.ImagePath);

        // Verify
        _mockProductRepository.Verify(p => p.GetByIdAsync(_productId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingProduct_ReturnsNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        _mockProductRepository.Setup(p => p.GetByIdAsync(nonExistingId))
            .ReturnsAsync((Product)null);

        var query = new GetProductByIdQuery(nonExistingId);

        // Act
        var result = await _getProductByIdHandler.Handle(query, CancellationToken.None);

        // Assert - Quando produto não existe, o handler retorna null (comportamento atual)
        Assert.Null(result);

        // Verify
        _mockProductRepository.Verify(p => p.GetByIdAsync(nonExistingId), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_Propagates()
    {
        // Arrange
        _mockProductRepository.Setup(p => p.GetByIdAsync(_productId))
            .ThrowsAsync(new Exception("Database error"));

        var query = new GetProductByIdQuery(_productId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _getProductByIdHandler.Handle(query, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_MappingIsCorrect()
    {
        // Arrange
        var query = new GetProductByIdQuery(_productId);

        // Act
        var result = await _getProductByIdHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_testProduct.Id, result.Id);
        Assert.Equal(_testProduct.Name, result.Name);
        Assert.Equal(_testProduct.Price, result.Price);
        Assert.Equal(_testProduct.AvailableQuantity, result.AvailableQuantity);
        Assert.Equal(_testProduct.ImagePath, result.ImagePath);
    }
}
