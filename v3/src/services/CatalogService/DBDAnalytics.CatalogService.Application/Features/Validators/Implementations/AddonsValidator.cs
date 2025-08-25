using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Validators.Implementations
{
    public sealed class AddonsValidator<T, TAddon> : AbstractValidator<T>
    where T : IHasAddons<TAddon>
    where TAddon : IHasName
    {
        public AddonsValidator(IValidator<TAddon> addonValidator, bool mustNotBeEmpty = false)
        {
            if (mustNotBeEmpty)
            {
                RuleFor(x => x.Addons)
                    .NotEmpty().WithMessage("Список аддонов не должен быть пустым.");
            }

            RuleForEach(x => x.Addons)
                .SetValidator(addonValidator);

            RuleFor(x => x.Addons)
                .Must(addons => addons.GroupBy(a => a.Name?.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты аддонов по имени в одном запросе.")
                .When(x => x.Addons != null && x.Addons.Any());
        }
    }
}