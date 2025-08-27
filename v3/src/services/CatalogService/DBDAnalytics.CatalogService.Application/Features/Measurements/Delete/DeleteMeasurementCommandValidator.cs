using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Delete
{
    public sealed class DeleteMeasurementCommandValidator : AbstractValidator<DeleteMeasurementCommand>
    {
        public DeleteMeasurementCommandValidator() => Include(new GuidValidator<DeleteMeasurementCommand>());
    }
}