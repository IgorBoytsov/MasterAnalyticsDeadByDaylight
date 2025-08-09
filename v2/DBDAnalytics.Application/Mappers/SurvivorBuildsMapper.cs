using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class SurvivorBuildsMapper
    {
        public static SurvivorBuildDTO ToDTO(this SurvivorBuildDomain survivorBuild)
        {
            return new SurvivorBuildDTO
            {
                Name = survivorBuild.Name,
                IdBuild = survivorBuild.IdBuild,
                IdItem = survivorBuild.IdItem,
                IdAddonItem1 = survivorBuild.IdAddonItem1,
                IdAddonItem2 = survivorBuild.IdAddonItem2,
                IdPerk1 = survivorBuild.IdPerk1,
                IdPerk2 = survivorBuild.IdPerk2,
                IdPerk3 = survivorBuild.IdPerk3,
                IdPerk4 = survivorBuild.IdPerk4,
            };
        }

        public static List<SurvivorBuildDTO> ToDTO(this IEnumerable<SurvivorBuildDomain> survivorBuilds)
        {
            var list = new List<SurvivorBuildDTO>();

            foreach (var survivorBuild in survivorBuilds)
            {
                list.Add(survivorBuild.ToDTO());
            }

            return list;
        }
    }
}