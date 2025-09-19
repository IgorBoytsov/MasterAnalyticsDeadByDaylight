using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Delete
{
    public sealed record DeleteOfferingCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}