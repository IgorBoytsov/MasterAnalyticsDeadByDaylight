using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Delete
{
    public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator() => Include(new IntValidator<DeleteRoleCommand>());
    }
}