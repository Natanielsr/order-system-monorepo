using FluentValidation;
using OrderSystem.Application.DTOs.Order;
using OrderSystem.Application.Orders.Commands.CreateOrder;

namespace OrderSystem.Application.Validator;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(o => o.OrderItems)
            .NotNull().WithMessage("product list can´t be null")
            .NotEmpty().WithMessage("product list can´t be empty");

        RuleFor(o => o.OrderItems)
            .Must(HasNoDuplicates)
            .WithMessage("the list contains duplicate product IDs.");

        RuleForEach(o => o.OrderItems)
            .Must(p => p.Quantity > 0).WithMessage("Product quantity must be bigger then zero");

        RuleForEach(o => o.OrderItems)
            .Must(oi => oi.ProductId != Guid.Empty).WithMessage("ProductId can´t be empty");
    }

    private bool HasNoDuplicates(List<CreateOrderItemDto> list)
    {
        // Compara o total de itens com o total de IDs únicos
        return list.Select(x => x.ProductId).Distinct().Count() == list.Count;
    }
}
