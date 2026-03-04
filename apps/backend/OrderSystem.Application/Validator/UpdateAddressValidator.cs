using FluentValidation;
using OrderSystem.Application.Addresses.Commands.UpdateAddress;

namespace OrderSystem.Application.Validator;

public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do endereço é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("O ID do endereço não pode ser vazio.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("O nome completo é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(IsValidCpf).WithMessage("O CPF informado é inválido.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("A rua é obrigatória.")
            .MaximumLength(150).WithMessage("A rua deve ter no máximo 150 caracteres.");

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
            .Matches(@"^\d{5}-\d{3}$|^\d{8}$").WithMessage("CEP em formato inválido (use 00000-000 ou 00000000).");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .NotEqual(Guid.Empty).WithMessage("O ID do usuário não pode ser um Guid vazio.");
    }

    // Método auxiliar para validar CPF
    private bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        // Remove todos os caracteres que não são dígitos
        var numericCpf = new string(cpf.Where(char.IsDigit).ToArray());

        // Deve ter exatamente 11 dígitos
        if (numericCpf.Length != 11) return false;

        // Aqui você pode adicionar o algoritmo completo de verificação de dígitos
        // Por enquanto, apenas valida o formato básico (11 dígitos numéricos)
        return true;
    }
}


