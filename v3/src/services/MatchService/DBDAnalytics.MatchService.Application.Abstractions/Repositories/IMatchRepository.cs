using DBDAnalytics.MatchService.Domain.Models;
using Shared.Api.Infrastructure.EFBase;

namespace DBDAnalytics.MatchService.Application.Abstractions.Repositories
{
    public interface IMatchRepository : IBaseRepository<Match>
    {
    }
}