using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Create
{
    public sealed class CreateTypeDeathCommandValidator : AbstractValidator<CreateTypeDeathCommand>
    {
        private readonly ITypeDeathRepository _typeDeathRepository;

        public CreateTypeDeathCommandValidator(ITypeDeathRepository typeDeathRepository)
        {
            _typeDeathRepository = typeDeathRepository;

            Include(new NameValidator<CreateTypeDeathCommand>(TypeDeathName.MAX_LENGTH));

            When(td => !string.IsNullOrWhiteSpace(td.Name), () =>
            {
                RuleFor(td => td.Name)
                    .MustAsync(async (name, clt) => !await _typeDeathRepository.Exist(name)).WithMessage("Тип смерти с таким названием уже существует.");
            });
        }
    }
}