using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete
{
    public sealed class DeleteWhoPlacedMapCommandValidator : AbstractValidator<DeleteWhoPlacedMapCommand>
    {
        public DeleteWhoPlacedMapCommandValidator() => Include(new IntValidator<DeleteWhoPlacedMapCommand>());
    }
}