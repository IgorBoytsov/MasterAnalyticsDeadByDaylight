using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.GetAll
{
    public sealed record GetAllPatchesQuery() : IRequest<List<PatchResponse>>;
}