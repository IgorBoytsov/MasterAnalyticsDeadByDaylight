using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class KillerBuildsMapper
    {
        public static KillerBuildDTO ToDTO(this KillerBuildDomain killerBuild)
        {
            return new KillerBuildDTO
            {
                Name = killerBuild.Name,
                IdBuild = killerBuild.IdBuild,
                IdKiller = killerBuild.IdKiller,
                IdAddon1 = killerBuild.IdAddon1,
                IdAddon2 = killerBuild.IdAddon2,
                IdPerk1 = killerBuild.IdPerk1,
                IdPerk2 = killerBuild.IdPerk2,
                IdPerk3 = killerBuild.IdPerk3,
                IdPerk4 = killerBuild.IdPerk4,
            };
        }

        public static List<KillerBuildDTO> ToDTO(this IEnumerable<KillerBuildDomain> killerBuilds)
        {
            var list = new List<KillerBuildDTO>();

            foreach (var killerBuild in killerBuilds)
            {
                list.Add(killerBuild.ToDTO());
            }

            return list;
        }
    }
}