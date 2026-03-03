using System.Data.Common;
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
    private readonly CreateOrderHandler createOrderHandler;
    private readonly IMapper _mapper;

    private Mock<IOrderUnitOfWork> mockOrderUnitOfWork;
    private Mock<IUserRepository> mockUserRepository;

    CancellationToken cancellationToken = new CancellationToken();

    private readonly List<Product> TestProducts = new List<Product>()
    {
        Product.CreateProduct("Product1", 1, 1, ""),
        Product.CreateProduct("Product2", 2, 2, ""),
        Product.CreateProduct("Product3", 3, 3, "")
    };

    Order? order;
    private readonly Guid OrderId = Guid.NewGuid();

    private readonly Guid userId = Guid.NewGuid();
    private User? user;

    public CreateOrderHandlerTest()
    {
        _mapper = mockAutoMapper();
        mockUnitOfWork();
        createMockOrder();
        createMockUserRepository();

        createOrderHandler = new CreateOrderHandler(mockOrderUnitOfWork!.Object, _mapper, mockUserRepository!.Object);
    }

    void createMockUserRepository()
    {
        mockUserRepository = new Mock<IUserRepository>();

        user = User.CreateUser("UserTest", "usertest@email.com", "password", "role", "telephone");

        mockUserRepository.Setup(ur => ur.GetByIdAsync(userId)).ReturnsAsync(user);

    }

    IMapper mockAutoMapper()
    {
        // 1. Configura o AutoMapper com seus Profiles reais
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderMappingProfile>(); // Substitua pelo seu Profile real
        });
        return config.CreateMapper();
    }

    void mockUnitOfWork()
    {
        mockOrderUnitOfWork = new Mock<IOrderUnitOfWork>();
        foreach (var product in TestProducts)
        {
            mockOrderUnitOfWork.Setup(m => m.productRepository.GetByIdAsync(product.Id)).ReturnsAsync(product);
        }

        mockOrderUnitOfWork.Setup(m => m.CommitAsync()).ReturnsAsync(true);
    }

    void createMockOrder()
    {
        order = new Order()
        {
            Id = OrderId,
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

        foreach (var product in TestProducts)
        {
            order.AddProductOrder(new OrderItem()
            {
                Id = product.Id,
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                OrderId = OrderId,
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = product.AvailableQuantity
            });
        }
        mockOrderUnitOfWork.Setup(m => m.orderRepository.AddAsync(It.IsAny<Order>())).ReturnsAsync(order);
    }

    CreateOrderCommand createOrderCommand()
    {
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>();
        foreach (var product in TestProducts)
        {
            createOrderProductDtos.Add(new() { ProductId = product.Id, Quantity = product.AvailableQuantity });
        }
        CreateOrderCommand command = new(createOrderProductDtos, userId, PaymentMethod.Pix, Guid.Empty);

        return command;
    }

    [Fact]
    public async Task CreateOrderHandlerSuccessTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await createOrderHandler.Handle(command, cancellationToken);

        //Assert
        Assert.Equal(OrderId, response.Id);

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Once);
        mockUserRepository.Verify(m => m.GetByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task CreateOrderProductIdTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await createOrderHandler.Handle(command, cancellationToken);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < TestProducts.Count; i++)
        {
            Product testProduct = TestProducts.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(testProduct.Id, responseProduct.ProductId);
        }

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderProductPriceTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        CreateOrderResponseDto response = await createOrderHandler.Handle(command, cancellationToken);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < TestProducts.Count; i++)
        {
            Product testProduct = TestProducts.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(testProduct.Price, responseProduct.UnitPrice);
        }

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderProductQuantityTest()
    {
        //Arrange
        CreateOrderCommand command = createOrderCommand();

        //Act
        var response = await createOrderHandler.Handle(command, cancellationToken);

        //Assert
        Assert.Equal(3, response.OrderItems.Count);
        for (int i = 0; i < TestProducts.Count; i++)
        {
            CreateOrderItemDto createOrderProductDto = command.OrderItems.ElementAt(i);
            var responseProduct = response.OrderItems.ElementAt(i);

            Assert.Equal(createOrderProductDto.Quantity, responseProduct.Quantity);
        }

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Exactly(3));
    }

    [Fact]
    public async Task CreateOrderHandlerWithProductNotFoundErrorTest()
    {
        //Arrange

        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = Guid.NewGuid(), Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, userId, PaymentMethod.Pix, Guid.Empty);

        //Act
        var er = await Assert.ThrowsAsync<ProductNotFoundException>(async () =>
        {
            var response = await createOrderHandler.Handle(command, cancellationToken);
        });

        Assert.Equal("Product Id in order doesn't exist", er.Message);

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task DuplicateProductsInOrderTest()
    {
        //Arrange

        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = TestProducts.ElementAt(0).Id, Quantity = 1 },
            new() { ProductId = TestProducts.ElementAt(0).Id, Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, userId, PaymentMethod.Pix, Guid.Empty);

        //Act
        var er = await Assert.ThrowsAsync<DuplicateProductInOrderException>(async () =>
        {
            var response = await createOrderHandler.Handle(command, cancellationToken);
        });

        Assert.Equal("Duplicate Product In Order", er.Message);

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ReduceStockProductsInOrderTest()
    {
        //Arrange
        var product = TestProducts.ElementAt(0);
        var productId = TestProducts.ElementAt(0).Id;
        List<CreateOrderItemDto> createOrderProductDtos = new List<CreateOrderItemDto>()
        {
            new() { ProductId = productId, Quantity = 1 }
        };

        CreateOrderCommand command = new(createOrderProductDtos, userId, PaymentMethod.Pix, Guid.Empty);

        //Act
        var response = await createOrderHandler.Handle(command, cancellationToken);

        //Assert
        Assert.Equal(0, product.AvailableQuantity);

        // BÔNUS: Verificar se o método foi chamado exatamente uma vez
        mockOrderUnitOfWork.Verify(m => m.productRepository.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        mockOrderUnitOfWork.Verify(m => m.orderRepository.AddAsync(It.IsAny<Order>()), Times.Once);
        mockOrderUnitOfWork.Verify(m => m.productRepository.UpdateAsync(productId, product), Times.Once);
    }

}
