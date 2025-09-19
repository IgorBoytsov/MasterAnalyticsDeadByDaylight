using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.GameModes.Delete
{
    public sealed record DeleteGameModeCommand(int Id) : IRequest<Result>,
        IHasIntId;
}