using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class PatchesMapper
    {
        public static PatchDTO ToDTO(this PatchDomain patch)
        {
            return new PatchDTO
            {
                IdPatch = patch.IdPatch,
                PatchNumber = patch.PatchNumber,
                PatchDateRelease = patch.PatchDateRelease,
            };
        }

        public static List<PatchDTO> ToDTO(this IEnumerable<PatchDomain> patches)
        {
            var list = new List<PatchDTO>();

            foreach (var patch in patches)
            {
                list.Add(patch.ToDTO());
            }

            return list;
        }
    }
}