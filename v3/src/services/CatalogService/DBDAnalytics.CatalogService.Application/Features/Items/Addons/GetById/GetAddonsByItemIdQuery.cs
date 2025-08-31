using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.GetById
{
    public sealed record GetAddonsByItemIdQuery(Guid Id) : IRequest<List<ItemAddonResponse>>,
        IHasGuidId;
}