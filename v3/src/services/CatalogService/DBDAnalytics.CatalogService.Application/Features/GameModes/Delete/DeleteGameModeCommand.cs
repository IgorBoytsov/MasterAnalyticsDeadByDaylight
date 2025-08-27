using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Delete
{
    public sealed record DeleteGameModeCommand(int Id) : IRequest<Result>,
        IHasIntId;
}