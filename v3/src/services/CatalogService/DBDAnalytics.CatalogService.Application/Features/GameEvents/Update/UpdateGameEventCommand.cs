using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Update
{
    public sealed record UpdateGameEventCommand(int GameEventId, string Name) : IRequest<Result>,
        IHasName;
}