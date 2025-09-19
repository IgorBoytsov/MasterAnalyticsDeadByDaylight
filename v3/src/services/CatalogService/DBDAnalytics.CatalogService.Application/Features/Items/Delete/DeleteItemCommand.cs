using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Delete
{
    public sealed record DeleteItemCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}