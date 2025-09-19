using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete
{
    public sealed record DeleteWhoPlacedMapCommand(int Id) : IRequest<Result>,
        IHasIntId;
}