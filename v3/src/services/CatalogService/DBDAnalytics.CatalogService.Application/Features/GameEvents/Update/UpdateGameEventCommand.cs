using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Update
{
    public sealed record UpdateGameEventCommand(int GameEventId, string Name) : IRequest<Result>,
        IHasName;
}