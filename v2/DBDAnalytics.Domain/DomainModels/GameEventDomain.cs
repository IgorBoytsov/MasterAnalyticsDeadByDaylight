namespace DBDAnalytics.Domain.DomainModels
{
    public class GameEventDomain
    {
        private GameEventDomain(int idGameEvent, string gameEventName, string? gameEventDescription)
        {
            IdGameEvent = idGameEvent;
            GameEventName = gameEventName;
            GameEventDescription = gameEventDescription;
        }

        public int IdGameEvent { get; private set; }

        public string GameEventName { get; private set; } = null!;

        public string? GameEventDescription { get; private set; }

        public static (GameEventDomain? GameEventDomain, string? Message) Create(int idGameEvent, string gameEventName, string? gameEventDescription)
        {
            string message = string.Empty;

            const int MaxGameEventNameLength = 20;
            const int MaxGameEventDescriptionLength = 1000;
            const string DefaultGameEventDescription = "Описание отсутствует.";

            if (string.IsNullOrWhiteSpace(gameEventName))
            {
                return (null, "Название ивента не может быть пустым.");
            }

            if (gameEventName.Length > MaxGameEventNameLength)
            {
                return (null, $"Название ивента не может превышать {MaxGameEventNameLength} символов.");
            }

            if (gameEventDescription?.Length > MaxGameEventDescriptionLength)
            {
                return (null, $"Слишком длинное описание! Максимально допустимая длинна не более {MaxGameEventDescriptionLength} символов.");
            }

            string finalGameEventDescription = string.IsNullOrWhiteSpace(gameEventDescription) ? DefaultGameEventDescription : gameEventDescription;

            var gameEvent = new GameEventDomain(idGameEvent, gameEventName, finalGameEventDescription);

            return (gameEvent, message);
        }
    }
}