using DBDAnalytics.Domain.DomainModels.DetailsMatchView;
using DBDAnalytics.Domain.DomainModels.DetailsModels;
using DBDAnalytics.Domain.Enums;
using DBDAnalytics.Domain.Interfaces.Repositories;
using DBDAnalytics.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.Infrastructure.Repositories
{
    public class DetailsMatchRepository(Func<DBDContext> contextFactory) : IDetailsMatchRepository
    {
        private readonly Func<DBDContext> _contextFactory = contextFactory;

        /// <summary>
        /// Описание соответствует перегружаемым методам.
        /// </summary>
        /// <param name="idEntity">Id Киллера, выжившего, карты и любой сущности по которой нужно выполнить фильтрацию.</param>
        /// <param name="idsMatches">Если нужно получить матчи по списку ID.</param>
        /// <param name="associations">Игровая ассоциация. Значение элементов равные Id в источники данных.</param>
        /// <param name="filterParameter">Определят по какой сущности нужно произвести фильтрацию.</param>
        /// <returns></returns>
        private async Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(int idEntity, List<int> idsMatches, Associations associations, FilterParameter filterParameter)
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
                            .AsQueryable();

                if (idEntity > 0)
                {
                    Action action = filterParameter switch
                    {
                        FilterParameter.Killers => () => query = query.Where(x => x.IdKillerNavigation.IdAssociation == (int)associations).Where(x => x.IdKillerNavigation.IdKiller == idEntity),
                        _ => () => throw new Exception("По такому параметру фильтрация не проводиться")
                    };
                    action?.Invoke();
                }
                else
                {
                    query = query.Where(x => idsMatches.Contains(x.IdGameStatistic));
                }

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

        public async Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(int idEntity, Associations associations, FilterParameter filterParameter)
        {
            return await GetDetailsMatch(idEntity, new List<int>(), associations, filterParameter);
        }

        public async Task<(List<DetailsMatchDomain> KillerDetails, int TotalMatch)> GetDetailsMatch(List<int> idsMatches)
        {
            return await GetDetailsMatch(-1, idsMatches, Associations.None, FilterParameter.None);
        }

        public async Task<DetailsMatchViewDomain> GetDetailsViewMatch(int idGameStatistic)
        {
            using (var _context = _contextFactory())
            {
                var query = _context.GameStatistics
                    .AsNoTracking()
                    .Where(x => x.IdGameStatistic == idGameStatistic)

                    // Включаем основные связанные сущности
                    .Include(x => x.IdKillerNavigation)
                        .ThenInclude(x => x.IdKillerNavigation)
                    .Include(x => x.IdKillerNavigation)
                        .ThenInclude(x => x.IdKillerOfferingNavigation)
                    .Include(x => x.IdKillerNavigation)
                        .ThenInclude(x => x.IdAssociationNavigation)
                    .Include(x => x.IdKillerNavigation)
                        .ThenInclude(x => x.IdPlatformNavigation)
                    .Include(x => x.IdMapNavigation)
                    .Include(x => x.IdGameEventNavigation)
                    .Include(x => x.IdGameModeNavigation)

                    // Включаем Выжившего 1 и его связанные данные
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(s => s.IdItemNavigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(s => s.IdAddon1Navigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(s => s.IdAddon2Navigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(x => x.IdSurvivorOfferingNavigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(x => x.IdAssociationNavigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(x => x.IdPlatformNavigation)
                    .Include(x => x.IdSurvivors1Navigation)
                        .ThenInclude(x => x.IdTypeDeathNavigation)

                    // Включаем Выжившего 2 и его связанные данные
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(s => s.IdItemNavigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(s => s.IdAddon1Navigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(s => s.IdAddon2Navigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(x => x.IdSurvivorOfferingNavigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(x => x.IdAssociationNavigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(x => x.IdPlatformNavigation)
                    .Include(x => x.IdSurvivors2Navigation)
                        .ThenInclude(x => x.IdTypeDeathNavigation)

                    // Включаем Выжившего 3 и его связанные данные
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(s => s.IdItemNavigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(s => s.IdAddon1Navigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(s => s.IdAddon2Navigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(x => x.IdSurvivorOfferingNavigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(x => x.IdAssociationNavigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(x => x.IdPlatformNavigation)
                    .Include(x => x.IdSurvivors3Navigation)
                        .ThenInclude(x => x.IdTypeDeathNavigation)

                    // Включаем Выжившего 4 и его связанные данные
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(s => s.IdItemNavigation)
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(s => s.IdAddon1Navigation)
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(s => s.IdAddon2Navigation)
                     .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(x => x.IdSurvivorOfferingNavigation)
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(x => x.IdAssociationNavigation)
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(x => x.IdPlatformNavigation)
                    .Include(x => x.IdSurvivors4Navigation)
                        .ThenInclude(x => x.IdTypeDeathNavigation)

                    .AsSplitQuery().AsQueryable(); 

                var domain = await query
                    .Select(x => 
                        DetailsMatchViewDomain.Create(
                            x.IdMapNavigation.MapImage,
                            x.IdMapNavigation.MapName,
                            x.IdGameEventNavigation.GameEventName,
                            x.IdGameModeNavigation.GameModeName,
                            x.DateTimeMatch,
                            x.GameTimeMatch,
                            x.ResultMatch,
                        DetailsMatchKillerViewDomain.Create(
                            x.IdKillerNavigation.IdKillerNavigation.KillerImage,
                            x.IdKillerNavigation.IdKillerNavigation.KillerAbilityImage,
                            x.IdKillerNavigation.IdKillerNavigation.KillerName,
                            x.IdKillerNavigation.Prestige,
                            x.IdKillerNavigation.KillerAccount,
                            x.IdKillerNavigation.AnonymousMode,
                            x.IdKillerNavigation.Bot,
                            x.IdKillerNavigation.IdAssociationNavigation.PlayerAssociationName,
                            x.IdKillerNavigation.IdPlatformNavigation.PlatformName,

                            x.IdKillerNavigation.IdAddon1Navigation.AddonImage, x.IdKillerNavigation.IdAddon1Navigation.AddonName,
                            x.IdKillerNavigation.IdAddon2Navigation.AddonImage, x.IdKillerNavigation.IdAddon2Navigation.AddonName,

                            x.IdKillerNavigation.IdPerk1Navigation.PerkImage, x.IdKillerNavigation.IdPerk1Navigation.PerkName,
                            x.IdKillerNavigation.IdPerk2Navigation.PerkImage, x.IdKillerNavigation.IdPerk2Navigation.PerkName,
                            x.IdKillerNavigation.IdPerk3Navigation.PerkImage, x.IdKillerNavigation.IdPerk3Navigation.PerkName,
                            x.IdKillerNavigation.IdPerk4Navigation.PerkImage, x.IdKillerNavigation.IdPerk4Navigation.PerkName,
                            
                            x.IdKillerNavigation.IdKillerOfferingNavigation.OfferingImage,
                            x.IdKillerNavigation.IdKillerOfferingNavigation.OfferingName),
                        DetailsMatchSurvivorViewDomain.Create(
                            x.IdSurvivors1Navigation.IdSurvivorNavigation.SurvivorImage,
                            x.IdSurvivors1Navigation.IdSurvivorNavigation.SurvivorName,
                            x.IdSurvivors1Navigation.Prestige,
                            x.IdSurvivors1Navigation.SurvivorAccount,
                            x.IdSurvivors1Navigation.AnonymousMode,
                            x.IdSurvivors1Navigation.Bot,
                            x.IdSurvivors1Navigation.IdAssociationNavigation.PlayerAssociationName,
                            x.IdSurvivors1Navigation.IdPlatformNavigation.PlatformName,
                            x.IdSurvivors1Navigation.IdTypeDeathNavigation.TypeDeathName,

                            x.IdSurvivors1Navigation.IdItemNavigation.ItemImage, x.IdSurvivors1Navigation.IdItemNavigation.ItemName,

                            x.IdSurvivors1Navigation.IdAddon1Navigation.ItemAddonImage, x.IdSurvivors1Navigation.IdAddon1Navigation.ItemAddonName,
                            x.IdSurvivors1Navigation.IdAddon2Navigation.ItemAddonImage, x.IdSurvivors1Navigation.IdAddon2Navigation.ItemAddonName,

                            x.IdSurvivors1Navigation.IdPerk1Navigation.PerkImage, x.IdSurvivors1Navigation.IdPerk1Navigation.PerkName,
                            x.IdSurvivors1Navigation.IdPerk2Navigation.PerkImage, x.IdSurvivors1Navigation.IdPerk2Navigation.PerkName,
                            x.IdSurvivors1Navigation.IdPerk3Navigation.PerkImage, x.IdSurvivors1Navigation.IdPerk3Navigation.PerkName,
                            x.IdSurvivors1Navigation.IdPerk4Navigation.PerkImage, x.IdSurvivors1Navigation.IdPerk4Navigation.PerkName,
                            
                            x.IdSurvivors1Navigation.IdSurvivorOfferingNavigation.OfferingImage,
                            x.IdSurvivors1Navigation.IdSurvivorOfferingNavigation.OfferingName),
                        DetailsMatchSurvivorViewDomain.Create(
                            x.IdSurvivors2Navigation.IdSurvivorNavigation.SurvivorImage,
                            x.IdSurvivors2Navigation.IdSurvivorNavigation.SurvivorName,
                            x.IdSurvivors2Navigation.Prestige,
                            x.IdSurvivors2Navigation.SurvivorAccount,
                            x.IdSurvivors2Navigation.AnonymousMode,
                            x.IdSurvivors2Navigation.Bot,
                            x.IdSurvivors2Navigation.IdAssociationNavigation.PlayerAssociationName,
                            x.IdSurvivors2Navigation.IdPlatformNavigation.PlatformName,
                            x.IdSurvivors2Navigation.IdTypeDeathNavigation.TypeDeathName,

                            x.IdSurvivors2Navigation.IdItemNavigation.ItemImage, x.IdSurvivors2Navigation.IdItemNavigation.ItemName,

                            x.IdSurvivors2Navigation.IdAddon1Navigation.ItemAddonImage, x.IdSurvivors2Navigation.IdAddon1Navigation.ItemAddonName,
                            x.IdSurvivors2Navigation.IdAddon2Navigation.ItemAddonImage, x.IdSurvivors2Navigation.IdAddon2Navigation.ItemAddonName,

                            x.IdSurvivors2Navigation.IdPerk1Navigation.PerkImage, x.IdSurvivors2Navigation.IdPerk1Navigation.PerkName,
                            x.IdSurvivors2Navigation.IdPerk2Navigation.PerkImage, x.IdSurvivors2Navigation.IdPerk2Navigation.PerkName,
                            x.IdSurvivors2Navigation.IdPerk3Navigation.PerkImage, x.IdSurvivors2Navigation.IdPerk3Navigation.PerkName,
                            x.IdSurvivors2Navigation.IdPerk4Navigation.PerkImage, x.IdSurvivors2Navigation.IdPerk4Navigation.PerkName,

                            x.IdSurvivors2Navigation.IdSurvivorOfferingNavigation.OfferingImage,
                            x.IdSurvivors2Navigation.IdSurvivorOfferingNavigation.OfferingName),
                        DetailsMatchSurvivorViewDomain.Create(
                            x.IdSurvivors3Navigation.IdSurvivorNavigation.SurvivorImage,
                            x.IdSurvivors3Navigation.IdSurvivorNavigation.SurvivorName,
                            x.IdSurvivors3Navigation.Prestige,
                            x.IdSurvivors3Navigation.SurvivorAccount,
                            x.IdSurvivors3Navigation.AnonymousMode,
                            x.IdSurvivors3Navigation.Bot,
                            x.IdSurvivors3Navigation.IdAssociationNavigation.PlayerAssociationName,
                            x.IdSurvivors3Navigation.IdPlatformNavigation.PlatformName,
                            x.IdSurvivors3Navigation.IdTypeDeathNavigation.TypeDeathName,

                            x.IdSurvivors3Navigation.IdItemNavigation.ItemImage, x.IdSurvivors3Navigation.IdItemNavigation.ItemName,

                            x.IdSurvivors3Navigation.IdAddon1Navigation.ItemAddonImage, x.IdSurvivors3Navigation.IdAddon1Navigation.ItemAddonName,
                            x.IdSurvivors3Navigation.IdAddon2Navigation.ItemAddonImage, x.IdSurvivors3Navigation.IdAddon2Navigation.ItemAddonName,

                            x.IdSurvivors3Navigation.IdPerk1Navigation.PerkImage, x.IdSurvivors3Navigation.IdPerk1Navigation.PerkName,
                            x.IdSurvivors3Navigation.IdPerk2Navigation.PerkImage, x.IdSurvivors3Navigation.IdPerk2Navigation.PerkName,
                            x.IdSurvivors3Navigation.IdPerk3Navigation.PerkImage, x.IdSurvivors3Navigation.IdPerk3Navigation.PerkName,
                            x.IdSurvivors3Navigation.IdPerk4Navigation.PerkImage, x.IdSurvivors3Navigation.IdPerk4Navigation.PerkName,

                            x.IdSurvivors3Navigation.IdSurvivorOfferingNavigation.OfferingImage,
                            x.IdSurvivors3Navigation.IdSurvivorOfferingNavigation.OfferingName),
                        DetailsMatchSurvivorViewDomain.Create(
                            x.IdSurvivors4Navigation.IdSurvivorNavigation.SurvivorImage,
                            x.IdSurvivors4Navigation.IdSurvivorNavigation.SurvivorName,
                            x.IdSurvivors4Navigation.Prestige,
                            x.IdSurvivors4Navigation.SurvivorAccount,
                            x.IdSurvivors4Navigation.AnonymousMode,
                            x.IdSurvivors4Navigation.Bot,
                            x.IdSurvivors4Navigation.IdAssociationNavigation.PlayerAssociationName,
                            x.IdSurvivors4Navigation.IdPlatformNavigation.PlatformName,
                            x.IdSurvivors4Navigation.IdTypeDeathNavigation.TypeDeathName,

                            x.IdSurvivors4Navigation.IdItemNavigation.ItemImage, x.IdSurvivors4Navigation.IdItemNavigation.ItemName,
                                         
                            x.IdSurvivors4Navigation.IdAddon1Navigation.ItemAddonImage, x.IdSurvivors4Navigation.IdAddon1Navigation.ItemAddonName,
                            x.IdSurvivors4Navigation.IdAddon2Navigation.ItemAddonImage, x.IdSurvivors4Navigation.IdAddon2Navigation.ItemAddonName,
                                         
                            x.IdSurvivors4Navigation.IdPerk1Navigation.PerkImage, x.IdSurvivors3Navigation.IdPerk1Navigation.PerkName,
                            x.IdSurvivors4Navigation.IdPerk2Navigation.PerkImage, x.IdSurvivors4Navigation.IdPerk2Navigation.PerkName,
                            x.IdSurvivors4Navigation.IdPerk3Navigation.PerkImage, x.IdSurvivors4Navigation.IdPerk3Navigation.PerkName,
                            x.IdSurvivors4Navigation.IdPerk4Navigation.PerkImage, x.IdSurvivors4Navigation.IdPerk4Navigation.PerkName,

                            x.IdSurvivors4Navigation.IdSurvivorOfferingNavigation.OfferingImage,
                            x.IdSurvivors4Navigation.IdSurvivorOfferingNavigation.OfferingName)
                        ))
                    .FirstOrDefaultAsync();

                return domain;
            }
        }
    }
}