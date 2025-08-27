using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Delete
{ 
    public sealed record DeleteRarityCommand(int Id) : IRequest<Result>,
        IHasIntId;
}