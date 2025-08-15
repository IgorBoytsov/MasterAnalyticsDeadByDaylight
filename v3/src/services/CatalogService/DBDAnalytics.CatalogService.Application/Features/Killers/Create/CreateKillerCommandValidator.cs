using DBDAnalytics.CatalogService.Application.Common.Repository;
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

            RuleForEach(x => x.Addons)
                .SetValidator(new CreateAddonCommandDataValidator());

            RuleForEach(x => x.Perks)
                .SetValidator(new CreatePerkCommandDataValidator());

            RuleFor(x => x.Addons)
                .Must(addons => addons.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты аддонов по имени в одном запросе.")
                .When(x => x.Addons != null && x.Addons.Any());

            RuleFor(x => x.Perks)
                .Must(perks => perks.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты перков по имени в одном запросе.")
                .When(x => x.Perks != null && x.Perks.Any());

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(k => k.Name)
                    .MustAsync(async (name, cancellationToken) => await _killerRepository.ExistName(name)).WithMessage($"Киллер с таким именем уже существует");
            });
        }
    }

    public sealed class CreateAddonCommandDataValidator : AbstractValidator<CreateAddonCommandData>
    {
        public CreateAddonCommandDataValidator()
        {
            Include(new NameValidator<CreateAddonCommandData>(KillerAddonName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateAddonCommandData>());

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                .NotEmpty().WithMessage("Имя файла улучшения не может быть пустым");

                RuleFor(x => x.Image!.Content)
                .NotEmpty().WithMessage("Содержимое файла улучшения не может быть пустым.");
            });
        }
    }

    public sealed class CreatePerkCommandDataValidator : AbstractValidator<CreatePerkCommandData>
    {
        public CreatePerkCommandDataValidator()
        {
            Include(new NameValidator<CreatePerkCommandData>(KillerPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreatePerkCommandData>());

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                .NotEmpty().WithMessage("Имя файла перка не может быть пустым");

                RuleFor(x => x.Image!.Content)
                .NotEmpty().WithMessage("Содержимое перка файла не может быть пустым.");
            });
        }
    }
}