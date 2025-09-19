using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameEvents.Delete
{
    public sealed record DeleteGameEventCommand(int Id) : IRequest<Result>,
        IHasIntId;
}