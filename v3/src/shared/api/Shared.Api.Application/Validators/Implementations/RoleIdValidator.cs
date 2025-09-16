using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class RoleIdValidator : AbstractValidator<IHasRoleId>
    {
        public RoleIdValidator()
            => RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("Идентификатор (Int) не может быть меньше 1.");
    }
}