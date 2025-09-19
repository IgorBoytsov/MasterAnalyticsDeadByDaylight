using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Update
{
    public sealed record UpdatePlatformCommand(int PlatformId, string Name) : IRequest<Result>,
        IHasName;
}