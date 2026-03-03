using System;
using FluentValidation;
using OrderSystem.Application.Products.Commands.CreateProduct;

namespace OrderSystem.Application.Validator;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        // Validating basic fields
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.AvailableQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Available quantity cannot be negative.");

        // Validating the File/Stream
        RuleFor(p => p.FileStream)
            .NotNull().WithMessage("Image file is required.")
            .Must(s => s.Length > 0).WithMessage("File cannot be empty.")
            .Must(s => s.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5MB.");

        RuleFor(p => p.FileName)
            .NotEmpty().WithMessage("File name is required.")
            .Must(HaveValidExtension).WithMessage("Invalid file extension. Only .jpg, .jpeg and .png are allowed.");

        RuleFor(p => p.ContentType)
            .NotEmpty().WithMessage("Content type is required.")
            .Must(c => c.StartsWith("image/")).WithMessage("Provided file is not a valid image.");
    }

    private bool HaveValidExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return new[] { ".jpg", ".jpeg", ".png" }.Contains(extension);
    }
}
