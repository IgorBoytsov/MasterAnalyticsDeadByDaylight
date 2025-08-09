using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    public static class MatchAttributesMapper
    {
        public static MatchAttributeDTO ToDTO(this MatchAttributeDomain matchAttribute)
        {
            return new MatchAttributeDTO
            {
                IdMatchAttribute = matchAttribute.IdMatchAttribute,
                AttributeName = matchAttribute.AttributeName,
                AttributeDescription = matchAttribute.AttributeDescription,
                CreatedAt = matchAttribute.CreatedAt,
                IsHide = matchAttribute.IsHide,
            };
        }

        public static List<MatchAttributeDTO> ToDTO(this IEnumerable<MatchAttributeDomain> matchAttributes)
        {
            var list = new List<MatchAttributeDTO>();

            foreach (var matchAttribute in matchAttributes)
            {
                list.Add(matchAttribute.ToDTO());
            }

            return list;
        }
    }
}