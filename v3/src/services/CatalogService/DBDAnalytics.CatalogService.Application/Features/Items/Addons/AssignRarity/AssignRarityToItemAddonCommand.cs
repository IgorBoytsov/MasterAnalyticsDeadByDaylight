using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.AssignRarity
{
    public sealed record AssignRarityToItemAddonCommand(Guid ItemId, Guid ItemAddonId, int? RarityId) : IRequest<Result>;
}