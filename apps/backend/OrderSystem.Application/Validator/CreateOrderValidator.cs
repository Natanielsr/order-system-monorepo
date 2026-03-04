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

        RuleForEach(o => o.OrderItems).ChildRules(item =>
        {
            item.RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Product quantity must be bigger then zero");

            item.RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId can´t be empty");
        });
    }

    private bool HasNoDuplicates(List<CreateOrderItemDto> list)
    {
        if (list == null) return true; // NotNull rule handles null case
        // Compara o total de itens com o total de IDs únicos
        return list.Select(x => x.ProductId).Distinct().Count() == list.Count;
    }
}
