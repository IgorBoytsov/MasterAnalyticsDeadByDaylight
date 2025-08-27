using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.RemoveAddon
{
    public sealed class DeleteKillerAddonCommandValidator : AbstractValidator<DeleteKillerAddonCommand>
    {
        public DeleteKillerAddonCommandValidator()
        {
            RuleFor(x => x.IdKiller)
                .NotEmpty().WithMessage("Идентификатор убийцы не может быть пустым.");

            RuleFor(x => x.IdAddon)
                .NotEmpty().WithMessage("Идентификатор улучшения не может быть пустым.");
        }
    }
}