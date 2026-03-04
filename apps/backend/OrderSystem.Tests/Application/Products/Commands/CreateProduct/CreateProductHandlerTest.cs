#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Product;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Products.Commands.CreateProduct;
using OrderSystem.Application.Services;
using OrderSystem.Application.Validator;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Products.Commands.CreateProduct;

public class CreateProductHandlerTest
{
    private readonly CreateProductHandler _createProductHandler;
    private readonly IMapper _mapper;

    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IStorageService> _mockStorageService;

    private readonly string _productName = "Test Product";
    private readonly decimal _price = 29.99m;
    private readonly int _availableQuantity = 50;
    private readonly string _fileName = "test.jpg";
    private readonly string _contentType = "image/jpeg";
    private readonly Stream _fileStream;

    private readonly Product _createdProduct;
    private readonly string _uploadedFilePath = "/images/products/test.jpg";

    public CreateProductHandlerTest()
    {
        // Configurar mocks
        _mockProductRepository = new Mock<IProductRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockStorageService = new Mock<IStorageService>();

        // Criar stream de teste (vazio)
        _fileStream = new MemoryStream(new byte[0]);

        // Criar produto esperado
        _createdProduct = Product.CreateProduct(
            _productName,
            _price,
            _availableQuantity,
            _uploadedFilePath
        );

        // Setup do storage service
        _mockStorageService.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), _fileName))
            .ReturnsAsync(_uploadedFilePath);

        // Setup do produto repository
        _mockProductRepository.Setup(p => p.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(_createdProduct);

        // Configurar AutoMapper
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductMappingProfile>();
        }).CreateMapper();

        // Instanciar handler
        _createProductHandler = new CreateProductHandler(
            _mockProductRepository.Object,
            _mockUnitOfWork.Object,
            _mapper,
            _mockStorageService.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesProductAndReturnsResponse()
    {
        // Arrange
        var command = new CreateProductCommand(
            _productName,
            _price,
            _availableQuantity,
            _fileStream,
            _fileName,
            _contentType
        );

        // Act
        var result = await _createProductHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_productName, result.Name);
        Assert.Equal(_price, result.Price);
        Assert.Equal(_availableQuantity, result.AvailableQuantity);
        Assert.Equal(_uploadedFilePath, result.ImagePath);

        // Verify
        _mockStorageService.Verify(s => s.UploadFileAsync(It.IsAny<Stream>(), _fileName), Times.Once);
        _mockProductRepository.Verify(p => p.AddAsync(It.IsAny<Product>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidImage_ThrowsNotValidImageException()
    {
        // Arrange
        var invalidStream = new MemoryStream(new byte[0]);
        var command = new CreateProductCommand(
            _productName,
            _price,
            _availableQuantity,
            invalidStream,
            "invalid.exe",
            "application/x-msdownload"
        );

        // Act & Assert
        await Assert.ThrowsAsync<NotValidImageException>(
            () => _createProductHandler.Handle(command, CancellationToken.None)
        );

        // Verify: storage e add não foram chamados
        _mockStorageService.Verify(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()), Times.Never);
        _mockProductRepository.Verify(p => p.AddAsync(It.IsAny<Product>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_StorageServiceThrowsException_Propagates()
    {
        // Arrange
        var command = new CreateProductCommand(
            _productName,
            _price,
            _availableQuantity,
            _fileStream,
            _fileName,
            _contentType
        );

        _mockStorageService.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Storage error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _createProductHandler.Handle(command, CancellationToken.None)
        );

        // Verify: add e commit não chamados
        _mockProductRepository.Verify(p => p.AddAsync(It.IsAny<Product>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ProductRepositoryThrowsException_Propagates()
    {
        // Arrange
        var command = new CreateProductCommand(
            _productName,
            _price,
            _availableQuantity,
            _fileStream,
            _fileName,
            _contentType
        );

        _mockProductRepository.Setup(p => p.AddAsync(It.IsAny<Product>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _createProductHandler.Handle(command, CancellationToken.None)
        );

        // Verify: commit não chamado
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_MapsProductCorrectly()
    {
        // Arrange
        var command = new CreateProductCommand(
            _productName,
            _price,
            _availableQuantity,
            _fileStream,
            _fileName,
            _contentType
        );

        // Act
        var result = await _createProductHandler.Handle(command, CancellationToken.None);

        // Assert - Verify all fields are mapped correctly
        Assert.Equal(_productName, result.Name);
        Assert.Equal(_price, result.Price);
        Assert.Equal(_availableQuantity, result.AvailableQuantity);
        Assert.NotNull(result.ImagePath);
    }
}
