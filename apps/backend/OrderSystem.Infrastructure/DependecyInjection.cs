using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderSystem.Application.Services;
using OrderSystem.Domain.Repository;
using OrderSystem.Domain.Services;
using OrderSystem.Domain.UnitOfWork;
using OrderSystem.Infrastructure.Data;
using OrderSystem.Infrastructure.Repository.EntityFramework;
using OrderSystem.Infrastructure.Services;
using OrderSystem.Infrastructure.UnitOfWork;

namespace OrderSystem.Infrastructure;

public static class DependecyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("OrderSystem.Infrastructure") // IMPORTANTE: Define onde as migrações serão salvas
            ));


        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrderUnitOfWork, OrderUnitOfWork>();
        services.AddScoped<IUnitOfWork, UnitOfWorkEf>();
        services.AddScoped<ITokenService, JWTTokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IStorageService, LocalStorageService>();
        services.AddScoped<IAddressRepository, AddressRepository>();

        //jwt 
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]!))
            };
        });

        return services;
    }
}
