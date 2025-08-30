using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Update
{
    public sealed record UpdatePlatformCommand(int PlatformId, string Name) : IRequest<Result>,
        IHasName;
}