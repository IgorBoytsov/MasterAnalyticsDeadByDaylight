namespace DBDAnalytics.Domain.DomainModels
{
    public class MapDomain
    {
        private MapDomain(int idMap, string mapName, byte[]? mapImage, string? mapDescription, int? idMeasurement)
        {
            IdMap = idMap;
            MapName = mapName;
            MapImage = mapImage;
            MapDescription = mapDescription;
            IdMeasurement = idMeasurement;
        }
        public int IdMap { get; private set; }

        public string MapName { get; private set; } = null!;

        public byte[]? MapImage { get; private set; }

        public string? MapDescription { get; private set; }

        public int? IdMeasurement { get; private set; }

        public static (MapDomain? MapDomain, string Message) Create(int idMap, string mapName, byte[]? mapImage, string? mapDescription, int? idMeasurement)
        {
            string message = string.Empty;
            const int MaxMapNameLength = 150;
            const int MaxMapDescriptionLength = 2000;

            if (string.IsNullOrWhiteSpace(mapName))
            {
                return (null, "Вы не указали название карты");
            }
                
            if (mapName.Length > MaxMapNameLength)
            {
                return (null, $"Максимально допустимая длинна название карты - {MaxMapNameLength}");
            }

            if (mapDescription?.Length > MaxMapDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна название карты - {MaxMapDescriptionLength}");
            }

            if (idMeasurement <= 0)
            {
                return (null, "Вы не выбрали измерение к которому относиться карта");
            }

            var map = new MapDomain(idMap, mapName, mapImage, mapDescription, idMeasurement);

            return (map, message);
        }
    }
}