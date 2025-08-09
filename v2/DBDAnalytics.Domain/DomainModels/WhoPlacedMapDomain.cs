namespace DBDAnalytics.Domain.DomainModels
{
    public class WhoPlacedMapDomain
    {
        private WhoPlacedMapDomain(int idWhoPlacedMap, string whoPlacedMapName, string? description)
        {
            IdWhoPlacedMap = idWhoPlacedMap;
            WhoPlacedMapName = whoPlacedMapName;
            Description = description;
        }
        public int IdWhoPlacedMap { get; private set; }

        public string WhoPlacedMapName { get; private set; } = null!;

        public string? Description { get; private set; }

        public static (WhoPlacedMapDomain? WhoPlacedMapDomain, string? Message) Create(int idWhoPlacedMap, string whoPlacedMapName, string? description)
        {
            string message = string.Empty;
            const int MaxDescriptionLength = 300;

            if (string.IsNullOrWhiteSpace(whoPlacedMapName))
            {
                return (null, "Вы забыли указать название.");
            } 

            if (description?.Length > MaxDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxDescriptionLength}");
            }

            var whoPlacedMap = new WhoPlacedMapDomain(idWhoPlacedMap, whoPlacedMapName, description);

            return (whoPlacedMap, message);
        }
    }
}