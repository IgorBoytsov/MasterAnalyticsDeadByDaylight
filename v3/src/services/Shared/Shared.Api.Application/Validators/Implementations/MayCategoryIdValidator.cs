using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class MayCategoryIdValidator<T> : AbstractValidator<T> where T : IMayHasCategoryId
    {
        public MayCategoryIdValidator()
            => When(x => x.CategoryId.HasValue, () =>
               {
                   RuleFor(x => x.CategoryId)
                    .GreaterThan(0).WithMessage("ID категории не может быть меньше 1");
               });
    }
}