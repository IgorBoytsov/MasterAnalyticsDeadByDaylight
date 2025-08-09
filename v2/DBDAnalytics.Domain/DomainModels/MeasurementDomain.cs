namespace DBDAnalytics.Domain.DomainModels
{
    public class MeasurementDomain
    {
        private MeasurementDomain(int idMeasurement, string measurementName, string? measurementDescription)
        {
            IdMeasurement = idMeasurement;
            MeasurementName = measurementName;
            MeasurementDescription = measurementDescription;
        }
        public int IdMeasurement { get; private set; }

        public string MeasurementName { get; private set; } = null!;

        public string? MeasurementDescription { get; private set; }

        public static (MeasurementDomain? MeasurementDomain, string? Message) Create(int idMeasurement, string measurementName, string? measurementDescription)
        {
            string message = string.Empty;
            const int MaxMeasurementNameLength = 300;
            const int MaxMeasurementDescriptionLength = 10000;

            if (string.IsNullOrWhiteSpace(measurementName))
            {
                return (null, "Укажите название измерению.");
            }

            if (measurementName.Length > MaxMeasurementNameLength)
            {
                return (null, $"Максимально допустимая длинна названия - {MaxMeasurementNameLength}");
            }

            if (measurementDescription?.Length > MaxMeasurementDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxMeasurementDescriptionLength}");
            }

            var measurement = new MeasurementDomain(idMeasurement, measurementName, measurementDescription);

            return (measurement, message);
        }
    }
}