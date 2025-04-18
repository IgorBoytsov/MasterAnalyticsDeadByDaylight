using DBDAnalytics.Application.DTOs.DetailsDTOs;
using DBDAnalytics.Domain.DomainModels.DetailsModels;

namespace DBDAnalytics.Application.Mappers
{
    public static class DetailsMatchMapper
    {
        public static DetailsMatchDTO ToDTO(this DetailsMatchDomain domain)
        {
            return new DetailsMatchDTO()
            {
               KillerDTO = domain.Killer.ToDTO(),

                IdGameStatistic = domain.IdGameStatistic,

                IdWhoPlaceMap = domain.IdWhoPlaceMap,
                IdWhoPlaceMapWin = domain.IdWhoPlaceMapWin,

                CountKill = domain.CountKill,
                CountHook = domain.CountHook,
                RecentGenerator = domain.RecentGenerator,
                DurationMatch = domain.DurationMatch,
                Date = domain.Date,
                
                FirstSurvivorInfo = domain.FirstSurvivorInfo?.ToDTO(),
                SecondSurvivorInfo = domain.SecondSurvivorInfo?.ToDTO(),
                ThirdSurvivorInfo = domain.ThirdSurvivorInfo?.ToDTO(),
                FourthSurvivorInfo = domain.FourthSurvivorInfo?.ToDTO(),
            };
        }

        public static DetailsMatchSurvivorDTO ToDTO(this DetailsMatchSurvivorDomain domain)
        {
            return new DetailsMatchSurvivorDTO()
            {
                IdSurvivor = domain.IdSurvivor,
                IdPlatform = domain.IdPlatform,
                IdTypeDeath = domain.IdTypeDeath,
                Anonymous = domain.Anonymous,
                Bot = domain.Bot
            };
        }

        public static DetailsMatchKillerDTO ToDTO(this DetailsMatchKillerDomain domain)
        {
            return new DetailsMatchKillerDTO()
            {
                IdKiller = domain.IdKiller,
                FirstAddonID = domain.FirstAddonID,
                SecondAddonID = domain.SecondAddonID,
                FirstPerkID = domain.FirstPerkID,
                SecondPerkID = domain.SecondPerkID,
                ThirdPerkID = domain.ThirdPerkID,
                FourthPerkID = domain.FourthPerkID,
                Score = domain.Score,
            };
        }
    }
}