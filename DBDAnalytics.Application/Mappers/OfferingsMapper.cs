using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class OfferingsMapper
    {
        public static OfferingDTO ToDTO(this OfferingDomain offering)
        {
            return new OfferingDTO
            {
                IdCategory = offering.IdCategory,
                IdOffering = offering.IdOffering,
                IdRarity = offering.IdRarity,
                IdRole = offering.IdRole,
                OfferingDescription = offering.OfferingDescription,
                OfferingImage = offering.OfferingImage,
                OfferingName = offering.OfferingName,
            };
        }

        public static List<OfferingDTO> ToDTO(this IEnumerable<OfferingDomain> offerings)
        {
            var list = new List<OfferingDTO>();

            foreach (var offering in offerings)
            {
                list.Add(offering.ToDTO());
            }

            return list;
        }
    }
}