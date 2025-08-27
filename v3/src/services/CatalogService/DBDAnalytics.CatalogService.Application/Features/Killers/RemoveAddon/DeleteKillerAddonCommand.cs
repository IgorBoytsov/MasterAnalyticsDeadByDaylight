using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.RemoveAddon
{
    public sealed record DeleteKillerAddonCommand(Guid IdKiller, Guid IdAddon) : IRequest<Result>;
}