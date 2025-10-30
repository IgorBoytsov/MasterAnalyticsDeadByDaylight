using DBDAnalytics.MatchService.Application.Abstractions.Models;

namespace DBDAnalytics.MatchService.Application.Abstractions.Contexts
{
    public interface IReadDbContext
    {
        IQueryable<MatchListItemView> MatchListItems { get; }
        IQueryable<MatchDetailsView> MatchDetails { get; }
    }
}