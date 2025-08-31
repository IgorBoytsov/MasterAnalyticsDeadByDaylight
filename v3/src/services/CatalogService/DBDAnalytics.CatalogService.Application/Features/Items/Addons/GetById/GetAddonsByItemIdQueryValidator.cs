using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.GetById
{
    public sealed class GetAddonsByItemIdQueryValidator : AbstractValidator<GetAddonsByItemIdQuery>
    {
        public GetAddonsByItemIdQueryValidator() => Include(new GuidValidator<GetAddonsByItemIdQuery>());
    }
}