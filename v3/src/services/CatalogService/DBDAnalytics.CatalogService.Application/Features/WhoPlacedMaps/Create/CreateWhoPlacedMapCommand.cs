using DBDAnalytics.Shared.Contracts.Responses.Maps;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Create
{
    public sealed record CreateWhoPlacedMapCommand(int OldId, string Name) : IRequest<Result<WhoPlacedMapResponse>>,
        IHasName;
}