namespace DBDAnalytics.Domain.DomainModels
{
    public class MeasurementWithMapsDomain
    {
        private MeasurementWithMapsDomain(int idMeasurement, string measurementName, string? measurementDescription, IEnumerable<MapDomain> maps)
        {
            IdMeasurement = idMeasurement;
            MeasurementName = measurementName;
            MeasurementDescription = measurementDescription;

            foreach (MapDomain map in maps)
                Maps.Add(map);
        }

        public int IdMeasurement { get; private set; }

        public string MeasurementName { get; private set; } = null!;

        public string? MeasurementDescription { get; private set; }

        public List<MapDomain> Maps { get; set; } = new List<MapDomain>();

        public static (MeasurementWithMapsDomain? MeasurementWithMapsDTO, string? Message) Create(int idMeasurement, string measurementName, string? measurementDescription, IEnumerable<MapDomain> maps)
        {
            string message = string.Empty;
                
            var measurementWithMaps = new MeasurementWithMapsDomain(idMeasurement, measurementName, measurementDescription, maps);

            return (measurementWithMaps, message);
        }
    }
}