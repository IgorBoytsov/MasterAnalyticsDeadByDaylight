using DBDAnalytics.Shared.Contracts.Responses.Maps;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create
{
    public sealed record CreateWhoPlacedMapCommand(int OldId, string Name) : IRequest<Result<WhoPlacedMapResponse>>,
        IHasName;
}