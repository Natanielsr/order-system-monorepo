#nullable disable

using AutoMapper;
using Moq;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Orders.Commands.CreateOrder;
using OrderSystem.Domain.Entities;
using OrderSystem.Domain.Exceptions;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.UnitOfWork;

namespace OrderSystem.Tests.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandlerTest
{
    private readonly CreateOrderHandler _handler;
    private readonly IMapper _mapper;

    private readonly Mock<IOrderUnitOfWork> _mockOrderUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepository;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _orderId = Guid.NewGuid();
    private readonly User _testUser;
    private readonly Order _testOrder;

    private readonly List<Product> _testProducts = new()
    {
        Product.CreateProduct("Product1", 1, 1, ""),
        Product.CreateProduct("Product2", 2, 2, ""),
        Product.CreateProduct("Product3", 3, 3, "")
    };

    public CreateOrderHandlerTest()
    {
        // Configure AutoMapper with real profiles
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>();
        }).CreateMapper();

        // Setup mock user repository
        _mockUserRepository = new Mock<IUserRepository>();
        _testUser = User.CreateUser(Guid.NewGuid(), "UserTest", "usertest@email.com", "password", "role", "telephone");
        _mockUserRepository.Setup(ur => ur.GetByIdAsync(_userId)).ReturnsAsync(_testUser);

        // Setup mock order unit of work
        _mockOrderUnitOfWork = new Mock<IOrderUnitOfWork>();

        foreach (var product in _testProducts)
        {
            _mockOrderUnitOfWork
                .Setup(m => m.productRepository.GetByIdAsync(product.Id))
                .ReturnsAsync(product);
        }

        _mockOrderUnitOfWork.Setup(m => m.CommitAsync()).ReturnsAsync(true);

        // Setup mock order with items from test products
        _testOrder = new Order()
        {
            Id = _orderId,
            CreationDate = DateTimeOffset.UtcNow,
            UpdateDate = DateTimeOffset.UtcNow,
            Active = true,
            OrderItems = new List<OrderItem>(),
            UserId = Guid.Empty,
            UserName = "userName",
            UserEmail = "userEmail",
            Total = 14,
            Status = OrderStatus.Pending,
            Code = "code",
            AddressId = Guid.Empty
        };

        foreach (var product in _testProducts)
        {
            _testOrder.AddProductOrder(new OrderItem()
            {
                Id = product.Id,
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                OrderId = _orderId,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = product.AvailableQuantity
            });
        }

        _mockOrderUnitOfWork
            .Setup(m => m.orderRepository.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(_testOrder);

        // Instantiate handler
        _handler = new CreateOrderHandler(_mockOrderUnitOfWork.Object, _mapper, _mockUserRepository.Object);
    }

    CreateOrderCommand createOrderCommand()
    {
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>();
        foreach (var product in _testProducts)
        {
            createOrderProductDtos.Add(new() { ProductId = product.Id, Quantity = product.AvailableQuantity });
        }
        CreateOrderCommand command = new(createOrderProductDtos, _userId, PaymentMethod.Pix, Guid.Empty);

        return command;
    }

    [Fact]
    public async Task CreateOrderHandlerSuccessTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(_orderId, response.Id);

        _mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Once);
        _mockUserRepository.Verify(m => m.GetByIdAsync(_userId), Times.Once);
    }

    [Fact]
    public async Task CreateOrderProductIdTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < _testProducts.Count; i++)
        {
            Product testProduct = _testProducts.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(testProduct.Id, responseProduct.ProductId);
        }

        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderProductPriceTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        CreateOrderResponseDto response = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < _testProducts.Count; i++)
        {
            Product testProduct = _testProducts.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(testProduct.Price, responseProduct.UnitPrice);
        }

        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderProductQuantityTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await _handler.Handle(command, CancellationToken.None);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < _testProducts.Count; i++)
        {
            CreateOrderItemDto createOrderProductDto = command.OrderItems.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(createOrderProductDto.Quantity, responseProduct.Quantity);
        }

        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderHandlerWithProductNotFoundErrorTest()
    {
        //Arrange
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = Guid.NewGuid(), Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, _userId, PaymentMethod.Pix, Guid.Empty);

        //Act
        var er = await Assert.ThrowsAsync<ProductNotFoundException>(async () =>
        {
            var response = await _handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("Product Id in order doesn't exist", er.Message);

        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task DuplicateProductsInOrderTest()
    {
        //Arrange
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = _testProducts.ElementAt(0).Id, Quantity = 1 },
            new() { ProductId = _testProducts.ElementAt(0).Id, Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, _userId, PaymentMethod.Pix, Guid.Empty);

        //Act
        var er = await Assert.ThrowsAsync<DuplicateProductInOrderException>(async () =>
        {
            var response = await _handler.Handle(command, CancellationToken.None);
        });

        Assert.Equal("Duplicate Product In Order", er.Message);

        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        _mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ReduceStockProductsInOrderTest()
    {
        // Arrange
        var product = _testProducts.ElementAt(0);
        var productId = product.Id;
        int initialQuantity = product.AvailableQuantity;
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = productId, Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, _userId, PaymentMethod.Pix, Guid.Empty);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert - Verify UpdateAsync was called with correct product state
        _mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Once);

        _mockOrderUnitOfWork.Verify(m => m.productRepository.UpdateAsync(
            productId,
            It.Is<Product>(p => p.AvailableQuantity == initialQuantity - 1 && p.Id == productId)
        ), Times.Once);

        Assert.Equal(initialQuantity - 1, product.AvailableQuantity);
    }
}
