using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.AddMap
{
    public sealed class CreateMapCommandValidator : AbstractValidator<CreateMapCommand>
    {
        public CreateMapCommandValidator()
        {
           RuleFor(x => x.Maps)
                .NotEmpty().WithMessage("Кол-во карт для измерения должно быть больше 0, что бы операция могла быть выполнена");

            RuleForEach(x => x.Maps)
                .SetValidator(new AddMapToMeasurementCommandDataValidator());

            RuleFor(x => x.Maps)
                .Must(perks => perks.GroupBy(m => m.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты карт по имени в одном запросе.")
                .When(x => x.Maps != null && x.Maps.Any());
        }
    }

    public sealed class AddMapToMeasurementCommandDataValidator : AbstractValidator<AddMapToMeasurementCommandData>
    {
        public AddMapToMeasurementCommandDataValidator()
        {
            Include(new NameValidator<AddMapToMeasurementCommandData>(MapName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<AddMapToMeasurementCommandData>());
            Include(new MayFileInputValidator());

            RuleFor(m => m.MeasurementId)
                .NotEmpty().WithMessage("Не было передано измерение, куда нужно добавлять карты.");
        }
    }
}