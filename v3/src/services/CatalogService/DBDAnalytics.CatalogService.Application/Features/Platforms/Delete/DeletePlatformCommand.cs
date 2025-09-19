using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Delete
{
    public sealed record DeletePlatformCommand(int Id) : IRequest<Result>,
        IHasIntId;
}
