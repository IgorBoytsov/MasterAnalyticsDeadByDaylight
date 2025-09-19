using DBDAnalytics.Shared.Contracts.Responses.Match;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Create
{
    public sealed record CreateGameEventCommand(int OldId, string Name) : IRequest<Result<GameEventResponse>>,
        IHasName;
}