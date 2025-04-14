using DBDAnalytics.Domain.DomainModels.DetailsMatchView;
using DBDAnalytics.Domain.DomainModels.DetailsModels;
using DBDAnalytics.Domain.Enums;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IDetailsMatchRepository
    {
        Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(int idEntity, Associations associations, FilterParameter filterParameter);
        Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(List<int> idsMatches);
        Task<DetailsMatchViewDomain> GetDetailsViewMatch(int idGameStatistic);
    }
}