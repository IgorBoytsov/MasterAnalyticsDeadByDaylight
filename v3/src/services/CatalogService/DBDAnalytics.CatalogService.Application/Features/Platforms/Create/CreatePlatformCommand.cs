using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Create
{
    public sealed record CreatePlatformCommand(int OldId, string Name) : IRequest<Result<PlatformResponse>>,
        IHasName;
}