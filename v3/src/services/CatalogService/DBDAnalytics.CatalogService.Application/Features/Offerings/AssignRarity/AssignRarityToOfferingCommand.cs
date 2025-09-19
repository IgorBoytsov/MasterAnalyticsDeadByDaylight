using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.AssignRarity
{
    public sealed record AssignRarityToOfferingCommand(Guid OfferingId, int RarityId) : IRequest<Result>;
}