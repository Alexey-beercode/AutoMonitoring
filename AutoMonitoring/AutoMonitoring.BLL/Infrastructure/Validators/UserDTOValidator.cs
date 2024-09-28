using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using FluentValidation;

namespace AutoMonitoring.BLL.Infrastructure.Validators;

public class UserDTOValidator : AbstractValidator<UserDTO>
{
    public UserDTOValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("Login is required.")
            .MinimumLength(5).WithMessage("Login must be at least 5 characters long.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}