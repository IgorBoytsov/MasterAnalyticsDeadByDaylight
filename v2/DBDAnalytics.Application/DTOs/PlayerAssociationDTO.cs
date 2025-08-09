using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class PlayerAssociationDTO : BaseDTO<PlayerAssociationDTO>
    {
        public int IdPlayerAssociation { get; set; }

        public string? PlayerAssociationName { get; set; }

        public string? PlayerAssociationDescription { get; set; }
    }
}
