using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Create
{
    public sealed record CreateRarityCommand(int OldId, string Name) : IRequest<Result<RarityResponse>>,
        IHasName;
}