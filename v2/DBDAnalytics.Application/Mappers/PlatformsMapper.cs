using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class PlatformsMapper
    {
        public static PlatformDTO ToDTO(this PlatformDomain platform)
        {
            return new PlatformDTO
            {
                IdPlatform = platform.IdPlatform,
                PlatformDescription = platform.PlatformDescription,
                PlatformName = platform.PlatformName,
            };
        }

        public static List<PlatformDTO> ToDTO(this IEnumerable<PlatformDomain> platforms)
        {
            var list = new List<PlatformDTO>();

            foreach (var platform in platforms)
            {
                list.Add(platform.ToDTO());
            }

            return list;
        }
    }
}