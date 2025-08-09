using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorBuildDTO : BaseDTO<SurvivorBuildDTO>
    {
        public int IdBuild { get; set; }

        public string? Name { get; set; }

        public int IdPerk1 { get; set; }

        public int IdPerk2 { get; set; }

        public int IdPerk3 { get; set; }

        public int IdPerk4 { get; set; }

        public int IdItem { get; set; }

        public int IdAddonItem1 { get; set; }

        public int IdAddonItem2 { get; set; }
    }
}
