using DBDAnalytics.Domain.DomainModels.DetailsModels;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class DetailsMatchRepository(Func<DBDContext> contextFactory) : IDetailsMatchRepository
    {
        private readonly Func<DBDContext> _contextFactory = contextFactory;

        public async Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(int idKiller, int idAssociation)
        {
            using (var _context = _contextFactory())
            {
                var query = _context.GameStatistics
                    .AsNoTracking()
                        .Include(x => x.IdKillerNavigation.IdKillerNavigation)
                        .Include(x => x.IdSurvivors1Navigation)
                        .Include(x => x.IdSurvivors2Navigation)
                        .Include(x => x.IdSurvivors3Navigation)
                        .Include(x => x.IdSurvivors4Navigation)
                            .AsSplitQuery()
                                .Where(x => x.IdKillerNavigation.IdAssociation == idAssociation)
                                .Where(x => x.IdKillerNavigation.IdKiller == idKiller)
                                    .AsQueryable();

                var list = await query
                    .Select(x => DetailsMatchDomain.Create(
                        DetailsMatchKillerDomain.Create(
                            x.IdKillerNavigation.IdKiller,
                            x.IdKillerNavigation.IdAddon1,
                            x.IdKillerNavigation.IdAddon2,
                            x.IdKillerNavigation.IdPerk1,
                            x.IdKillerNavigation.IdPerk2,
                            x.IdKillerNavigation.IdPerk3,
                            x.IdKillerNavigation.IdPerk4,
                            x.IdKillerNavigation.KillerAccount),
                        x.IdGameStatistic,
                        x.CountKills,
                        x.CountHooks,
                        x.NumberRecentGenerators,
                        x.IdKillerNavigation.KillerAccount,
                        x.GameTimeMatch,
                        x.DateTimeMatch,
                        DetailsMatchSurvivorDomain.Create(
                            x.IdSurvivors1Navigation.IdSurvivorNavigation.IdSurvivor,
                            x.IdSurvivors1Navigation.IdTypeDeathNavigation.IdTypeDeath,
                            x.IdSurvivors1Navigation.IdPlatformNavigation.IdPlatform,
                            x.IdSurvivors1Navigation.Bot,
                            x.IdSurvivors1Navigation.AnonymousMode),
                        DetailsMatchSurvivorDomain.Create(
                            x.IdSurvivors2Navigation.IdSurvivorNavigation.IdSurvivor,
                            x.IdSurvivors2Navigation.IdTypeDeathNavigation.IdTypeDeath,
                            x.IdSurvivors2Navigation.IdPlatformNavigation.IdPlatform,
                            x.IdSurvivors2Navigation.Bot,
                            x.IdSurvivors2Navigation.AnonymousMode),
                        DetailsMatchSurvivorDomain.Create(
                            x.IdSurvivors3Navigation.IdSurvivorNavigation.IdSurvivor,
                            x.IdSurvivors3Navigation.IdTypeDeathNavigation.IdTypeDeath,
                            x.IdSurvivors3Navigation.IdPlatformNavigation.IdPlatform,
                            x.IdSurvivors3Navigation.Bot,
                            x.IdSurvivors3Navigation.AnonymousMode),
                        DetailsMatchSurvivorDomain.Create(
                            x.IdSurvivors4Navigation.IdSurvivorNavigation.IdSurvivor,
                            x.IdSurvivors4Navigation.IdTypeDeathNavigation.IdTypeDeath,
                            x.IdSurvivors4Navigation.IdPlatformNavigation.IdPlatform,
                            x.IdSurvivors4Navigation.Bot,
                            x.IdSurvivors4Navigation.AnonymousMode)
                    ))
                    .ToListAsync();

                var totalMatch = _context.GameStatistics.Count(x => x.IdKillerNavigation.IdAssociation == 1);

                return (list, totalMatch);
            }
        }

    }
}