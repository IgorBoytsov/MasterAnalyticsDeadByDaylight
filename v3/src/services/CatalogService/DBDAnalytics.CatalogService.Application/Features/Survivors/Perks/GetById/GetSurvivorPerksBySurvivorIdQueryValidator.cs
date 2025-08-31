using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.GetById
{
    public sealed class GetSurvivorPerksBySurvivorIdQueryValidator : AbstractValidator<GetSurvivorPerksBySurvivorIdQuery>
    {
        public GetSurvivorPerksBySurvivorIdQueryValidator() => Include(new GuidValidator<GetSurvivorPerksBySurvivorIdQuery>());
    }
}