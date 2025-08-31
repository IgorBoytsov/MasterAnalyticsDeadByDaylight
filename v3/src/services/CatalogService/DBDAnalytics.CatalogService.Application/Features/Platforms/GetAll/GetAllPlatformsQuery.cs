using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Platforms.GetAll
{
    public sealed record GetAllPlatformsQuery() : IRequest<List<PlatformResponse>>;
}