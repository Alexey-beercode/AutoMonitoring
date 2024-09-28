using AutoMonitoring.BLL.DTOs.Implementations.Requests.User;
using FluentValidation;

namespace AutoMonitoring.BLL.Infrastructure.Validators;

public class BlockUserDTOValidator : AbstractValidator<BlockUserDTO>
{
    public BlockUserDTOValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.BlockUntil)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.BlockUntil.HasValue)
            .WithMessage("BlockUntil must be a future date.");
    }
}