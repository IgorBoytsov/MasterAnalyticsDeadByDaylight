using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class MustCategoryIdValidator<T> : AbstractValidator<T> where T : IMustHasCategoryId
    {
        public MustCategoryIdValidator()
            => RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Идентификатор (Int) не может быть меньше 1.");
    }
}