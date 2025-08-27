using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Delete
{
    public sealed record DeleteSurvivorCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}