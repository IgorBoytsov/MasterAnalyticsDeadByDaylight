using DBDAnalytics.Shared.Contracts.Responses.Match;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Create
{
    public sealed record CreateGameModeCommand(int OldId, string Name) : IRequest<Result<GameModeResponse>>,
        IHasName;
}