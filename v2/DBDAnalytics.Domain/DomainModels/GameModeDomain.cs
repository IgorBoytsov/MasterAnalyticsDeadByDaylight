namespace DBDAnalytics.Domain.DomainModels
{
    public class GameModeDomain
    {
        private GameModeDomain(int idGameMode, string gameModeName, string? gameModeDescription)
        {
            IdGameMode = idGameMode;
            GameModeName = gameModeName;
            GameModeDescription = gameModeDescription;
        }

        public int IdGameMode { get; private set; }

        public string GameModeName { get; private set; } = null!;

        public string? GameModeDescription { get; private set; }

        public static (GameModeDomain? GameModeDomain, string? Message) Create(int idGameMode, string gameModeName, string? gameModeDescription)
        {
            string message = string.Empty;

            const int MaxGameModeNameLength = 100;
            const int MaxGameModeDescriptionLength = 1000;
            const string DefaultGameModeDescription = "Описание отсутствует.";

            if (string.IsNullOrWhiteSpace(gameModeName))
            {
                return (null, "Название режима не может быть пустым.");
            }

            if (gameModeName.Length > MaxGameModeNameLength)
            {
                return (null, $"Название режима не может превышать {MaxGameModeNameLength} символов.");
            }

            if (gameModeDescription?.Length > MaxGameModeDescriptionLength)
            {
                return (null, $"Слишком длинное описание! Максимально допустимая длинна не более {MaxGameModeDescriptionLength} символов.");
            }

            string finalGameModeDescription = string.IsNullOrWhiteSpace(gameModeDescription) ? DefaultGameModeDescription : gameModeDescription;

            var gameMode = new GameModeDomain(idGameMode, gameModeName, finalGameModeDescription);

            return (gameMode, message);
        }
    }
}