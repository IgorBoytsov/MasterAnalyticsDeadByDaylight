using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.GetById
{
    public sealed record GetPerksByKillerIdQuery(Guid Id) : IRequest<List<KillerPerkResponse>>,
        IHasGuidId;
}