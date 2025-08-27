using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.WhoPlacedMaps.Delete
{
    public sealed record DeleteWhoPlacedMapCommand(int Id) : IRequest<Result>,
        IHasIntId;
}