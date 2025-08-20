using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Create
{
    public sealed record CreateRarityCommand(int OldId, string Name) : IRequest<Result<RarityResponse>>,
        IHasName;
}