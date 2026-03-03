using Microsoft.EntityFrameworkCore;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Data;

namespace OrderSystem.Tests.Infrastructure;

public class DbContextTest
{
    [Fact]
    public async Task ShouldThrowAnExceptionWhenThereIsConcurrency()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context1 = new AppDbContext(options);
        using var context2 = new AppDbContext(options);

        var productId = new Guid("9f3b2c7e-6a41-4d8b-b5a2-3c9e1f7a8d42");
        // 🔹 Cria o banco
        using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            context.Products.Add(new Product()
            {
                Id = productId,
                CreationDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow,
                Active = true,
                ImagePath = "",
                Name = "ProductTests",
                Price = 999,
                AvailableQuantity = 1,
            });

            await context.SaveChangesAsync();
        }

        Product? p1 = await context1.Products.FindAsync(productId);
        Product? p2 = await context2.Products.FindAsync(productId);

        p1!.ReduceInStock(1);
        await context1.SaveChangesAsync();

        p2!.ReduceInStock(1);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => context2.SaveChangesAsync());
    }
}
