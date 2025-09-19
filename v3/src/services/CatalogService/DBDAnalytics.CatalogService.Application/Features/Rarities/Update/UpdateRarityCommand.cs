using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Update
{
    public sealed record UpdateRarityCommand(int RarityId, string Name) : IRequest<Result>,
        IHasName;
}