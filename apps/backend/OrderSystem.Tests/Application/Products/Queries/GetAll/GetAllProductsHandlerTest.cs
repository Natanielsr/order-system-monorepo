#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Products.Queries.GetAll;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Repository;

namespace OrderSystem.Tests.Application.Products.Queries.GetAll;

public class GetAllProductsHandlerTest
{
    private readonly GetAllProductsHandler _getAllProductsHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IProductRepository> _mockProductRepository;

    private readonly List<Product> _testProducts;

    public GetAllProductsHandlerTest()
    {
        // Configurar mocks
        _mockProductRepository = new Mock<IProductRepository>();

        // Criar dados de teste
        _testProducts = new List<Product>()
        {
            Product.CreateProduct("Product 1", 10.00m, 100, "/images/products/1.jpg"),
            Product.CreateProduct("Product 2", 20.00m, 50, "/images/products/2.jpg"),
            Product.CreateProduct("Product 3", 30.00m, 25, "/images/products/3.jpg")
        };

        _mockProductRepository.Setup(p => p.GetAllAsync())
            .ReturnsAsync(_testProducts);

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _getAllProductsHandler = new GetAllProductsHandler(
            _mockProductRepository.Object,
            _mapper
        );
    }

    [Fact]
    public async Task Handle_ReturnsAllProducts()
    {
        // Act
        var result = await _getAllProductsHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        // Verificar mapeamento
        Assert.Equal("Product 1", result[0].Name);
        Assert.Equal(10.00m, result[0].Price);
        Assert.Equal(100, result[0].AvailableQuantity);
        Assert.Equal("/images/products/1.jpg", result[0].ImagePath);

        Assert.Equal("Product 2", result[1].Name);
        Assert.Equal(20.00m, result[1].Price);

        Assert.Equal("Product 3", result[2].Name);
        Assert.Equal(30.00m, result[2].Price);

        // Verify
        _mockProductRepository.Verify(p => p.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _mockProductRepository.Setup(p => p.GetAllAsync())
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _getAllProductsHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        // Verify
        _mockProductRepository.Verify(p => p.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_Propagates()
    {
        // Arrange
        _mockProductRepository.Setup(p => p.GetAllAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _getAllProductsHandler.Handle(new GetAllProductsQuery(), CancellationToken.None)
        );
    }

    [Fact]
    public async Task Handle_MappingIsCorrect()
    {
        // Act
        var result = await _getAllProductsHandler.Handle(new GetAllProductsQuery(), CancellationToken.None);

        // Assert - Verificar que o DTMapper foi chamado e os dados são mapeados corretamente
        Assert.All(result, item =>
        {
            Assert.NotNull(item.Name);
            Assert.True(item.Price >= 0);
            Assert.True(item.AvailableQuantity >= 0);
        });

        _mockProductRepository.Verify(p => p.GetAllAsync(), Times.Once);
    }
}
