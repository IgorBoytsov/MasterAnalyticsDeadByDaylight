using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Create
{
    public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleCommandValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

            Include(new NameValidator<CreateRoleCommand>(RoleName.MAX_LENGTH));

            When(r => !string.IsNullOrWhiteSpace(r.Name), () =>
            {
                RuleFor(r => r.Name)
                    .MustAsync(async (name, clt) => !await _roleRepository.Exist(name)).WithMessage("Роль с таким названием уже существует.");
            });
        }
    }
}