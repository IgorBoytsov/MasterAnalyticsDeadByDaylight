using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Validators.Implementations
{
    public class PerksValidator<T, TPerk> : AbstractValidator<T>
    where T : IHasPerks<TPerk>
    where TPerk : IHasName
    {
        public PerksValidator(IValidator<TPerk> perkValidator, bool mustNotBeEmpty = false)
        {
            if (mustNotBeEmpty)
            {
                RuleFor(x => x.Perks)
                    .NotEmpty().WithMessage("Список перков не должен быть пустым.");
            }

            RuleForEach(x => x.Perks)
                .SetValidator(perkValidator);

            RuleFor(x => x.Perks)
                .Must(perks => perks.GroupBy(p => p.Name?.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты перков по имени в одном запросе.")
                .When(x => x.Perks != null && x.Perks.Any());
        }
    }
}