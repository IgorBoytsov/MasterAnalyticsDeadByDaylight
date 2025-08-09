using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class TypeDeathsMapper
    {
        public static TypeDeathDTO ToDTO(this TypeDeathDomain typeDeath)
        {
            return new TypeDeathDTO
            {
                IdTypeDeath = typeDeath.IdTypeDeath,
                TypeDeathDescription = typeDeath.TypeDeathDescription,
                TypeDeathName = typeDeath.TypeDeathName,
            };
        }

        public static List<TypeDeathDTO> ToDTO(this IEnumerable<TypeDeathDomain> typeDeaths)
        {
            var list = new List<TypeDeathDTO>();

            foreach (var typeDeath in typeDeaths)
            {
                list.Add(typeDeath.ToDTO());
            }

            return list;
        }
    }
}