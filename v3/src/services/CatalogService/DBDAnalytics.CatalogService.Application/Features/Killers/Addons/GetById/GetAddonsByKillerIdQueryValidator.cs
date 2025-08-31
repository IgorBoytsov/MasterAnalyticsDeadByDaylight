using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetById
{
    public sealed class GetAddonsByKillerIdQueryValidator : AbstractValidator<GetAddonsByKillerIdQuery>
    {
        public GetAddonsByKillerIdQueryValidator() => Include(new GuidValidator<GetAddonsByKillerIdQuery>());
    }
}