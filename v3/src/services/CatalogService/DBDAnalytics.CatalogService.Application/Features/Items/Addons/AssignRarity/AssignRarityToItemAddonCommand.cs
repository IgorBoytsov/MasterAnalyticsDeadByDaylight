using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.AssignRarity
{
    public sealed record AssignRarityToItemAddonCommand(Guid ItemId, Guid ItemAddonId, int? RarityId) : IRequest<Result>;
}