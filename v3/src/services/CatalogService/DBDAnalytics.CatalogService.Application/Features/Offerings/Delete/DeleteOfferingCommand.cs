using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Delete
{
    public sealed record DeleteOfferingCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}