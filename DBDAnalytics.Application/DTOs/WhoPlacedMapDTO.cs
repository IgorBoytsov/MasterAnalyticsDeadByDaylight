using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class WhoPlacedMapDTO : BaseDTO<WhoPlacedMapDTO>
    {
        public int IdWhoPlacedMap { get; set; }

        public string WhoPlacedMapName { get; set; } = null!;
    }
}
