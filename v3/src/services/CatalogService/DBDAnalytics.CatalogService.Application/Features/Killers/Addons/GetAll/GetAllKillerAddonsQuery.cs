using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.GetAll
{
    public sealed record GetAllKillerAddonsQuery() : IRequest<List<KillerAddonResponse>>;
}