using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Update
{
    public sealed class UpdateMeasurementCommandValidator : AbstractValidator<UpdateMeasurementCommand>
    {
        public UpdateMeasurementCommandValidator() => Include(new NameValidator<UpdateMeasurementCommand>(MeasurementName.MAX_LENGTH));
    }
}