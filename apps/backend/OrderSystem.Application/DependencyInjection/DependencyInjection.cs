using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderSystem.Application.Behaviors;
using OrderSystem.Application.Mappings;
using OrderSystem.Application.Orders.Commands.CreateOrder;
using OrderSystem.Application.Orders.Queries.ListOrders;
using OrderSystem.Application.Users.Commands.Auth;
using OrderSystem.Application.Validator;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        // Registra todos os validators do assembly
        services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

        // Isso fará o AutoMapper procurar por qualquer classe que herde de 'Profile' 
        // no assembly onde o seu MappingProfile (ou qualquer classe da Application) está.
        services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
        services.AddAutoMapper(typeof(AddressMappingProfile).Assembly);

        // Configura o MediatR e adiciona o Behavior
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(ListOrdersQuery).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(AuthCommand).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
