namespace DBDAnalytics.Domain.DomainModels
{
    public class WhoPlacedMapDomain
    {
        private WhoPlacedMapDomain(int idWhoPlacedMap, string whoPlacedMapName)
        {
            IdWhoPlacedMap = idWhoPlacedMap;
            WhoPlacedMapName = whoPlacedMapName;
        }
        public int IdWhoPlacedMap { get; private set; }

        public string WhoPlacedMapName { get; private set; } = null!;

        public static (WhoPlacedMapDomain? WhoPlacedMapDomain, string? Message) Create(int idWhoPlacedMap, string whoPlacedMapName)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(whoPlacedMapName))
                return (null, "Вы забыли указать название.");

            var whoPlacedMap = new WhoPlacedMapDomain(idWhoPlacedMap, whoPlacedMapName);

            return (whoPlacedMap, message);
        }
    }
}