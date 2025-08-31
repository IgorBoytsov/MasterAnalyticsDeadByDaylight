using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Update
{
    public sealed class UpdateSurvivorPerkCommandValidator : AbstractValidator<UpdateSurvivorPerkCommand>
    {
        public UpdateSurvivorPerkCommandValidator()
        {
            Include(new NameValidator<UpdateSurvivorPerkCommand>(SurvivorPerkName.MAX_LENGTH));
            Include(new MayFileInputValidator());
            Include(new SemanticImageNameValidator<UpdateSurvivorPerkCommand>());
        }
    }
}