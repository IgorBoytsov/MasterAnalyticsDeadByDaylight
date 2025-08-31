using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetById
{
    public sealed record GetAddonsByKillerIdQuery(Guid Id) : IRequest<List<KillerAddonResponse>>,
        IHasGuidId;
}