using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.RemovePerk
{
    public sealed class DeleteKillerPerkCommandValidator : AbstractValidator<DeleteKillerPerkCommand>
    {
        public DeleteKillerPerkCommandValidator()
        {
            RuleFor(x => x.KillerId)
                .NotEmpty().WithMessage("Идентификатор убийцы не может быть пустым.");

            RuleFor(x => x.KillerPerkId)
                .NotEmpty().WithMessage("Идентификатор перка не может быть пустым.");
        }
    }
}