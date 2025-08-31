using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.GetById
{
    public sealed class GetMapsByMeasurementIdQueryValidator : AbstractValidator<GetMapsByMeasurementIdQuery>
    {
        public GetMapsByMeasurementIdQueryValidator() => Include(new GuidValidator<GetMapsByMeasurementIdQuery>());
    }
}