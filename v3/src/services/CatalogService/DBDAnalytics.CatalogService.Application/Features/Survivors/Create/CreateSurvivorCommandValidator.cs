using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Create
{
    public sealed class CreateSurvivorCommandValidator : AbstractValidator<CreateSurvivorCommand>
    {
        private readonly ISurvivorRepository _survivorRepository;

        public CreateSurvivorCommandValidator(ISurvivorRepository survivorRepository)
        {
            _survivorRepository = survivorRepository;

            Include(new NameValidator<CreateSurvivorCommand>(SurvivorName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateSurvivorCommand>());
            Include(new PerksValidator<CreateSurvivorCommand, CreateSurvivorPerkCommandData>(new CreateSurvivorPerkCommandDataValidator()));

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(k => k.Name)
                    .MustAsync(async (name, cancellationToken) => !await _survivorRepository.Exist(name)).WithMessage($"Выживший с таким именем уже существует");
            });
        }
    }

    public sealed class CreateSurvivorPerkCommandDataValidator : AbstractValidator<CreateSurvivorPerkCommandData>
    {
        public CreateSurvivorPerkCommandDataValidator()
        {
            Include(new NameValidator<CreateSurvivorPerkCommandData>(SurvivorPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateSurvivorPerkCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}