using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class PerkIdValidator<T> : AbstractValidator<T> where T : IHasPerkId
    {
        public PerkIdValidator()
            => RuleFor(x => x.PerkId)
                .NotEmpty().WithMessage("Идентификатор (Guid) не может быть пустым.");
    }
}