using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.GetAll
{
    public sealed record GetAllKillersQuery() : IRequest<List<KillerSoloResponse>>;
}