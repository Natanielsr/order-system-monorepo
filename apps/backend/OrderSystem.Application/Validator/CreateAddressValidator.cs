using FluentValidation;
using OrderSystem.Application.Addresses.Commands.CreateAddress;

public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(IsValidCpf).WithMessage("O CPF informado é inválido.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("A rua é obrigatória.")
            .MaximumLength(150);

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("O número é obrigatório.");

        RuleFor(x => x.Complement)
            .MaximumLength(100);

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("O bairro é obrigatório.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("A cidade é obrigatória.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("O estado é obrigatório.")
            .Length(2).WithMessage("Use a sigla do estado (ex: SP).");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{5}-\d{3}$|^\d{8}$").WithMessage("CEP em formato inválido (ex: 00000-000).");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("O ID do usuário não pode ser um Guid vazio.");
    }

    // Método auxiliar para validar CPF (Lógica simplificada)
    private bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        // Remove caracteres não numéricos
        var pos = cpf.Replace(".", "").Replace("-", "");
        if (pos.Length != 11) return false;

        // Aqui você pode adicionar o algoritmo completo de verificação de dígitos
        return true;
    }
}