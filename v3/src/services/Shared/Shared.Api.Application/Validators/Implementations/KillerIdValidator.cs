using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class KillerIdValidator<T> : AbstractValidator<T> where T : IHasKillerId 
    {
        public KillerIdValidator()
            => RuleFor(x => x.KillerId)
                .NotEmpty().WithMessage("Идентификатор (Guid) не может быть пустым.");
    }
}