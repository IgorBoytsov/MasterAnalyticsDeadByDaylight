using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Delete
{
    public sealed record DeletePlatformCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
