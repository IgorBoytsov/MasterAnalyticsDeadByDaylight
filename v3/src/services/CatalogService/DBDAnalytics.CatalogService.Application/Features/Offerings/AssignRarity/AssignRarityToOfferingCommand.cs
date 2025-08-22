using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignRarity
{
    public sealed record AssignRarityToOfferingCommand(Guid OfferingId, int RarityId) : IRequest<Result>;
}