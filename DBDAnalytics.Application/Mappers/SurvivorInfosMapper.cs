using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class SurvivorInfosMapper
    {
        public static SurvivorInfoDTO ToDTO(this SurvivorInfoDomain survivorInfo)
        {
            return new SurvivorInfoDTO
            {
                IdSurvivorInfo = survivorInfo.IdSurvivorInfo,
                IdSurvivor = survivorInfo.IdSurvivor,
                IdPerk1 = survivorInfo.IdPerk1,
                IdPerk2 = survivorInfo.IdPerk2,
                IdPerk3 = survivorInfo.IdPerk3,
                IdPerk4 = survivorInfo.IdPerk4,
                IdTypeDeath = survivorInfo.IdTypeDeath,
                IdAssociation = survivorInfo.IdAssociation,
                IdPlatform = survivorInfo.IdPlatform,
                IdSurvivorOffering = survivorInfo.IdSurvivorOffering,
                IdItem = survivorInfo.IdItem,
                IdAddon1 = survivorInfo.IdAddon1,
                IdAddon2 = survivorInfo.IdAddon2,
                AnonymousMode = survivorInfo.AnonymousMode,
                Bot = survivorInfo.Bot,
                Prestige = survivorInfo.Prestige,
                SurvivorAccount = survivorInfo.SurvivorAccount
            };
        }

        public static List<SurvivorInfoDTO> ToDTO(this IEnumerable<SurvivorInfoDomain> survivorInfos)
        {
            var list = new List<SurvivorInfoDTO>();

            foreach (var survivorInfo in survivorInfos)
            {
                list.Add(survivorInfo.ToDTO());
            }

            return list;
        }
    }
}
