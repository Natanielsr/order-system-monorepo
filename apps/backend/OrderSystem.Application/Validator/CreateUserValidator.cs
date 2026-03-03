using System;
using FluentValidation;
using OrderSystem.Application.Users.Commands.CreateUser;

namespace OrderSystem.Application.Validator;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Username)
            .NotNull().WithMessage("username can´t be null")
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
            .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("Username can only contain letters, numbers, dots (.), and underscores (_).")
            .Must(NotStartOrEndWithSpecialChar).WithMessage("Username cannot start or end with special characters.");

        RuleFor(u => u.Email)
            .NotNull().WithMessage("email can´t be null")
            .NotEmpty().WithMessage("email can´t be empty")
            .EmailAddress().WithMessage("Invalid email");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\!\?\*\.\@\#\$\%\^\-]").WithMessage("Password must contain at least one special character (!?*.@#$%^-).");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match.");

    }

    private bool NotStartOrEndWithSpecialChar(string username)
    {
        if (string.IsNullOrEmpty(username)) return true;

        char first = username[0];
        char last = username[username.Length - 1];

        return char.IsLetterOrDigit(first) && char.IsLetterOrDigit(last);
    }
}
