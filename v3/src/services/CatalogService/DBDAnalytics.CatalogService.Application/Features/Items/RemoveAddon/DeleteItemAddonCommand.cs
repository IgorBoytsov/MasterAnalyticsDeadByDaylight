using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Items.RemoveAddon
{
    public sealed record DeleteItemAddonCommand(Guid IdItem, Guid IdAddon) : IRequest<Result>;
}