using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorInfoDTO : BaseDTO<SurvivorInfoDTO>
    {
        public int IdSurvivorInfo { get; set; }

        public int IdSurvivor { get; set; }

        public int? IdPerk1 { get; set; }

        public int? IdPerk2 { get; set; }

        public int? IdPerk3 { get; set; }

        public int? IdPerk4 { get; set; }

        public int? IdItem { get; set; }

        public int? IdAddon1 { get; set; }

        public int? IdAddon2 { get; set; }

        public int IdTypeDeath { get; set; }

        public int IdAssociation { get; set; }

        public int IdPlatform { get; set; }

        public int? IdSurvivorOffering { get; set; }

        public int Prestige { get; set; }

        public bool Bot { get; set; }

        public bool AnonymousMode { get; set; }

        public int SurvivorAccount { get; set; }
    }
}