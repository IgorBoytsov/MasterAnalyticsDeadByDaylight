using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;
using Shared.Api.Application.Validators.Abstractions;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.Create
{
    public sealed record CreatePlatformCommand(int OldId, string Name) : IRequest<Result<PlatformResponse>>,
        IHasName;
}