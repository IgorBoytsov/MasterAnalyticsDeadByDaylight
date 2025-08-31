using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetById
{
    public sealed class GetPerksByKillerIdQueryValidator : AbstractValidator<GetPerksByKillerIdQuery>
    {
        public GetPerksByKillerIdQueryValidator() => Include(new GuidValidator<GetPerksByKillerIdQuery>());
    }
}