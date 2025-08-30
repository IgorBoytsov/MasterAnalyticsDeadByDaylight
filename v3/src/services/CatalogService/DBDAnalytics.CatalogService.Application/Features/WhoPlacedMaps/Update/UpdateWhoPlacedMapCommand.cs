using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update
{
    public sealed record UpdateWhoPlacedMapCommand(int WhoPlacedMapId, string Name) : IRequest<Result>,
        IHasName;
}