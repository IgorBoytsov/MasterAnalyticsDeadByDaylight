using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete
{
    public sealed record DeleteGameEventCommand(int Id) : IRequest<Result>,
        IHasIntId;
}