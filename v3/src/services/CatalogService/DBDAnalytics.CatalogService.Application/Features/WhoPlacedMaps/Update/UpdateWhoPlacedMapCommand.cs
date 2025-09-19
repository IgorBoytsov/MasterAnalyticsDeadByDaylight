using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Update
{
    public sealed record UpdateWhoPlacedMapCommand(int WhoPlacedMapId, string Name) : IRequest<Result>,
        IHasName;
}