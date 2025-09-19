using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Delete
{
    public sealed record DeleteKillerCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}