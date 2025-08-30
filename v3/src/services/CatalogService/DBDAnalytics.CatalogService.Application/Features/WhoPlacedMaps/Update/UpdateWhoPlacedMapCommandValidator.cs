using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update
{
    public sealed class UpdateWhoPlacedMapCommandValidator : AbstractValidator<UpdateWhoPlacedMapCommand>
    {
        public UpdateWhoPlacedMapCommandValidator() => Include(new NameValidator<UpdateWhoPlacedMapCommand>(PlacedMapName.MAX_LENGTH));
    }
}