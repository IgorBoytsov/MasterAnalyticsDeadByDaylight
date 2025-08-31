using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.GetById
{
    public sealed record GetSurvivorPerksBySurvivorIdQuery(Guid Id) : IRequest<List<SurvivorPerkResponse>>,
        IHasGuidId;
}