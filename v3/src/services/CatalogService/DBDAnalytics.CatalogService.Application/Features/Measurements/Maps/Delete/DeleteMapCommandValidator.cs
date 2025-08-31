using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Delete
{
    public sealed class DeleteMapCommandValidator : AbstractValidator<DeleteMapCommand>
    {
        public DeleteMapCommandValidator()
        {
            RuleFor(x => x.MeasurementId)
                .NotEmpty().WithMessage("Идентификатор измерения не может быть пустым.");

            RuleFor(x => x.MapId)
                .NotEmpty().WithMessage("Идентификатор карты не может быть пустым.");
        }
    }
}