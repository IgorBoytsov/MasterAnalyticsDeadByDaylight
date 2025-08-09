using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class MeasurementDTO : BaseDTO<MeasurementDTO>
    {
        public int IdMeasurement { get; set; }

        public string MeasurementName { get; set; } = null!;

        public string? MeasurementDescription { get; set; }
    }
}