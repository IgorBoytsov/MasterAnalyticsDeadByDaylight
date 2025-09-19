using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Delete
{
    public sealed record DeleteKillerPerkCommand(Guid KillerId, Guid KillerPerkId) : IRequest<Result>;
}