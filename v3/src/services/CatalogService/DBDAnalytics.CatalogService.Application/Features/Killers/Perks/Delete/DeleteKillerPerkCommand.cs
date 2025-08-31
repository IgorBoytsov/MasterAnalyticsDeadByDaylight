using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Perks.Delete
{
    public sealed record DeleteKillerPerkCommand(Guid KillerId, Guid KillerPerkId) : IRequest<Result>;
}