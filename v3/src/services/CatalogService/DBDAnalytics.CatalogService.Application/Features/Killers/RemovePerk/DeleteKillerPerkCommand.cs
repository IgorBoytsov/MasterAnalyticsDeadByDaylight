using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.RemovePerk
{
    public sealed record DeleteKillerPerkCommand(Guid KillerId, Guid KillerPerkId) : IRequest<Result>;
}