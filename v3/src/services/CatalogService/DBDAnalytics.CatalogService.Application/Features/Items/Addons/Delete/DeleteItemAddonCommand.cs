using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Addons.Delete
{
    public sealed record DeleteItemAddonCommand(Guid IdItem, Guid IdAddon) : IRequest<Result>;
}