using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Update
{
    public sealed record UpdateGameModeCommand(int GameModeId, string Name) : IRequest<Result>,
        IHasName;
}