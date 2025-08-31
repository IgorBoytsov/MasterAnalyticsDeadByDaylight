using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.GetAll
{
    public sealed record GetAllGameEventsQuery() : IRequest<List<GameEventResponse>>;
}