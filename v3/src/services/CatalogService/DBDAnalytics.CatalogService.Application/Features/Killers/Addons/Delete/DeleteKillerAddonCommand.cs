using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Delete
{
    public sealed record DeleteKillerAddonCommand(Guid IdKiller, Guid IdAddon) : IRequest<Result>;
}