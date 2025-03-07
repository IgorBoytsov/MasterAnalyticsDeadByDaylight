using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class PlayerAssociationsMapper
    {
        public static PlayerAssociationDTO ToDTO(this PlayerAssociationDomain playerAssociation)
        {
            return new PlayerAssociationDTO
            {
                IdPlayerAssociation = playerAssociation.IdPlayerAssociation,
                PlayerAssociationName = playerAssociation.PlayerAssociationName,
                PlayerAssociationDescription = playerAssociation.PlayerAssociationDescription,
            };
        }

        public static List<PlayerAssociationDTO> ToDTO(this IEnumerable<PlayerAssociationDomain> playerAssociations)
        {
            var list = new List<PlayerAssociationDTO>();

            foreach (var playerAssociation in playerAssociations)
            {
                list.Add(playerAssociation.ToDTO());
            }

            return list;
        }
    }
}
