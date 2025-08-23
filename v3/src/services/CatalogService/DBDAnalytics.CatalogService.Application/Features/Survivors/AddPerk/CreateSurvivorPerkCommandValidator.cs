using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.AddPerk
{
    public sealed class CreateSurvivorPerkCommandValidator : AbstractValidator<CreateSurvivorPerkCommand>
    {
        public CreateSurvivorPerkCommandValidator()
        {
            RuleFor(x => x.Perks)
                .NotEmpty().WithMessage("Кол-во перков для выжившего должно быть больше 0, что бы операция могла быть выполнена");

            RuleForEach(x => x.Perks)
                .SetValidator(new CreateSurvivorPerkCommandDataValidator());

            RuleFor(x => x.Perks)
                .Must(perks => perks.GroupBy(a => a.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты перков по имени в одном запросе.")
                .When(x => x.Perks != null && x.Perks.Any());
        }
    }

    public sealed class CreateSurvivorPerkCommandDataValidator : AbstractValidator<AddSurvivorPerkCommandData>
    {
        public CreateSurvivorPerkCommandDataValidator()
        {
            Include(new NameValidator<AddSurvivorPerkCommandData>(SurvivorPerkName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddSurvivorPerkCommandData>());
        }
    }
}