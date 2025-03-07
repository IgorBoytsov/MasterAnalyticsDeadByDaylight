using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class MapDTO : BaseDTO<MapDTO>
    {
        public int IdMap { get; set; }

        public string MapName { get; set; } = null!;

        public byte[]? MapImage { get; set; }

        public string? MapDescription { get; set; }

        public int? IdMeasurement { get; set; }

    }
}