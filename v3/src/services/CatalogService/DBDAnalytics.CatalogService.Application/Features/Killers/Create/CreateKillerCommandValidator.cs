using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Create
{
    public sealed class CreateKillerCommandValidator : AbstractValidator<CreateKillerCommand>
    {
        private readonly IKillerRepository _killerRepository;

        public CreateKillerCommandValidator(IKillerRepository killerRepository)
        {
            _killerRepository = killerRepository;

            Include(new NameValidator<CreateKillerCommand>(KillerName.MAX_LENGTH));
            Include(new AddonsValidator<CreateKillerCommand, CreateAddonCommandData>(new CreateAddonCommandDataValidator()));
            Include(new PerksValidator<CreateKillerCommand, CreatePerkCommandData>(new CreatePerkCommandDataValidator()));

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(k => k.Name)
                    .MustAsync(async (name, cancellationToken) => !await _killerRepository.ExistName(name)).WithMessage($"Киллер с таким именем уже существует");
            });
        }
    }

    public sealed class CreateAddonCommandDataValidator : AbstractValidator<CreateAddonCommandData>
    {
        public CreateAddonCommandDataValidator()
        {
            Include(new NameValidator<CreateAddonCommandData>(KillerAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateAddonCommandData>());
            Include(new MayFileInputValidator());
        }
    }

    public sealed class CreatePerkCommandDataValidator : AbstractValidator<CreatePerkCommandData>
    {
        public CreatePerkCommandDataValidator()
        {
            Include(new NameValidator<CreatePerkCommandData>(KillerPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreatePerkCommandData>());
            Include(new MayFileInputValidator());
        }
    }
}