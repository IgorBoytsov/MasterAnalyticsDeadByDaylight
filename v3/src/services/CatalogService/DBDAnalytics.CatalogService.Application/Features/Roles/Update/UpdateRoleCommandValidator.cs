using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Update
{
    public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator() => Include(new NameValidator<UpdateRoleCommand>(RoleName.MAX_LENGTH));
    }
}