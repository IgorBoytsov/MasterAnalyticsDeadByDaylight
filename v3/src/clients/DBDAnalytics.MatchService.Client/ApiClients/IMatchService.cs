using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using Shared.HttpClients.Abstractions;

namespace DBDAnalytics.MatchService.Client.ApiClients
{
    public interface IMatchService : IBaseWriteApiService<MatchDetailResponse, string>, IBaseReadApiService<MatchDetailResponse, string>
    {
    }
}