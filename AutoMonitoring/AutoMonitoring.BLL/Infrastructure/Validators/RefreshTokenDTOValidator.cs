using AutoMonitoring.BLL.DTOs.Implementations.Requests.Token;
using FluentValidation;

namespace AutoMonitoring.BLL.Infrastructure.Validators;

public class RefreshTokenDTOValidator : AbstractValidator<RefreshTokenDTO>
{
    public RefreshTokenDTOValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");

        RuleFor(x => x.DeviceName)
            .NotEmpty().WithMessage("Device name is required.");
    }
}