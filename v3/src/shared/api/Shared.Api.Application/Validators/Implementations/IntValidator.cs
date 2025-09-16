using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class IntValidator<T> : AbstractValidator<T> where T : IHasIntId
    {
        public IntValidator() 
            => RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Идентификатор (Int) должен быть больше 0");
    }
}