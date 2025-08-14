using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class GuidValidator<T> : AbstractValidator<T> where T : IHasGuidId
    {
        public GuidValidator()
            => RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Идентификатор (GUID) не может быть пустым");
    }
}