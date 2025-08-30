using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Update
{
    public sealed record UpdateGameModeCommand(int GameModeId, string Name) : IRequest<Result>,
        IHasName;
}