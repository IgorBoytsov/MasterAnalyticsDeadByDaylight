using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Items.Delete
{
    public sealed record DeleteItemCommand(Guid Id) : IRequest<Result>,
        IHasGuidId;
}