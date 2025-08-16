using DBDAnalytics.Shared.Contracts.Responses.Match;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Create
{
    public sealed record CreateGameEventCommand(int OldId, string Name) : IRequest<Result<GameEventResponse>>,
        IHasName;
}