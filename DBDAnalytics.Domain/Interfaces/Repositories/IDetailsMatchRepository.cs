using DBDAnalytics.Domain.DomainModels.DetailsMatchView;
using DBDAnalytics.Domain.DomainModels.DetailsModels;

namespace DBDAnalytics.Domain.Interfaces.Repositories
{
    public interface IDetailsMatchRepository
    {
        Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(int idKiller, int idAssociation);
        Task<DetailsMatchViewDomain> GetDetailsViewMatch(int idGameStatistic);
    }
}