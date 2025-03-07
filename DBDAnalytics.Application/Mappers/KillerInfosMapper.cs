using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillerInfosMapper
    {
        public static KillerInfoDTO ToDTO(this KillerInfoDomain killerInfo)
        {
            return new KillerInfoDTO
            {
                IdKillerInfo = killerInfo.IdKillerInfo,
                IdKiller = killerInfo.IdKiller,
                IdPerk1 = killerInfo.IdPerk1,
                IdPerk2 = killerInfo.IdPerk2,
                IdPerk3 = killerInfo.IdPerk3,
                IdPerk4 = killerInfo.IdPerk4,
                IdAddon1 = killerInfo.IdAddon1,
                IdAddon2 = killerInfo.IdAddon2,
                IdKillerOffering = killerInfo.IdKillerOffering,
                IdAssociation = killerInfo.IdAssociation,
                IdPlatform = killerInfo.IdPlatform,
                Bot = killerInfo.Bot,
                AnonymousMode = killerInfo.AnonymousMode,
                KillerAccount = killerInfo.KillerAccount,
                Prestige = killerInfo.Prestige,
            };
        }

        public static List<KillerInfoDTO> ToDTO(this IEnumerable<KillerInfoDomain> killerInfos)
        {
            var list = new List<KillerInfoDTO>();

            foreach (var killerInfo in killerInfos)
            {
                list.Add(killerInfo.ToDTO());
            }

            return list;
        }
    }
}
