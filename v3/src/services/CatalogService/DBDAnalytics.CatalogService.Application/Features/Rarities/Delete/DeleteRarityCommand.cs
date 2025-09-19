using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Delete
{ 
    public sealed record DeleteRarityCommand(int Id) : IRequest<Result>,
        IHasIntId;
}