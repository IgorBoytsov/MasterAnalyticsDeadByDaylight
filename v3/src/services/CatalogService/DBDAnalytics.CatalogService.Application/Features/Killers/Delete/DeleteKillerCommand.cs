using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Delete
{
    public sealed record DeleteKillerCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}