using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Update
{
    public sealed record UpdateRarityCommand(int RarityId, string Name) : IRequest<Result>,
        IHasName;
}