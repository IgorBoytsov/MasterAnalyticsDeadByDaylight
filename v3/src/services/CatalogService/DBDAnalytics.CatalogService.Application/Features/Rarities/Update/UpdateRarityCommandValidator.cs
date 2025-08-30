using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Update
{
    public sealed class UpdateRarityCommandValidator : AbstractValidator<UpdateRarityCommand>
    {
        public UpdateRarityCommandValidator() => Include(new NameValidator<UpdateRarityCommand>(RarityName.MAX_LENGTH));
    }
}