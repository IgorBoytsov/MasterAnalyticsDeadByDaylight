using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerBuildDTO : BaseDTO<KillerBuildDTO>
    {
        public int IdBuild { get; set; }

        public int IdKiller { get; set; }

        public string? Name { get; set; }

        public int IdPerk1 { get; set; }

        public int IdPerk2 { get; set; }

        public int IdPerk3 { get; set; }

        public int IdPerk4 { get; set; }

        public int IdAddon1 { get; set; }

        public int IdAddon2 { get; set; }
    }
}