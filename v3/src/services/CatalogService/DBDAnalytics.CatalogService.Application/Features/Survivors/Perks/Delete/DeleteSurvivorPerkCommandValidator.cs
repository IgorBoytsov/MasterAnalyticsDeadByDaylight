using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Delete
{
    public sealed class DeleteSurvivorPerkCommandValidator : AbstractValidator<DeleteSurvivorPerkCommand>
    {
        public DeleteSurvivorPerkCommandValidator()
        {
            RuleFor(x => x.SurvivorId)
                .NotEmpty().WithMessage("Идентификатор выжившего не может быть пустым.");

            RuleFor(x => x.PerkId)
                .NotEmpty().WithMessage("Идентификатор перка не может быть пустым.");
        }
    }
}