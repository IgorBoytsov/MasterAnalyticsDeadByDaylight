using DBDAnalytics.Shared.Contracts.Responses.MatchService;
using Shared.HttpClients;

namespace DBDAnalytics.MatchService.Client.ApiClients
{
    public sealed class MatchApiService(HttpClient client) 
        : BaseApiService<MatchDetailResponse, string>(client, "api/matches"), IMatchService
    {
    }
}