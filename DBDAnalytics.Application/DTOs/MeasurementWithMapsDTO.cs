using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.DTOs
{
    public class MeasurementWithMapsDTO
    {
        public int IdMeasurement { get; set; }

        public string MeasurementName { get; set; } = null!;

        public string? MeasurementDescription { get; set; }

        public ObservableCollection<MapDTO> Maps { get; set; } = new ObservableCollection<MapDTO>();
    }
}