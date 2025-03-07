namespace DBDAnalytics.Domain.DomainModels
{
    public class GameStatisticDomain
    {
        private GameStatisticDomain(
            int idGameStatistic, int idKiller, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, int idSurvivors1, int idSurvivors2, int idSurvivors3, int idSurvivors4,
            DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string? descriptionGame, byte[]? resultMatch, int idMatchAttribute)
        {
            IdGameStatistic = idGameStatistic;
            IdKiller = idKiller;
            IdMap = idMap;
            IdWhoPlacedMap = idWhoPlacedMap;
            IdWhoPlacedMapWin = idWhoPlacedMapWin;
            IdPatch = idPatch;
            IdGameMode = idGameMode;
            IdGameEvent = idGameEvent;
            IdSurvivor1 = idSurvivors1;
            IdSurvivor2 = idSurvivors2;
            IdSurvivor3 = idSurvivors3;
            IdSurvivor4 = idSurvivors4;
            DateTimeMatch = dateTimeMatch;
            GameTimeMatch = gameTimeMatch;
            CountKills = countKills;
            CountHooks = countHooks;
            NumberRecentGenerators = numberRecentGenerators;
            DescriptionGame = descriptionGame;
            ResultMatch = resultMatch;
            IdMatchAttribute = idMatchAttribute;
        }

        public int IdGameStatistic { get; private set; }

        public int IdKiller { get; private set; }

        public int IdMap { get; private set; }

        public int IdWhoPlacedMap { get; private set; }

        public int IdWhoPlacedMapWin { get; private set; }

        public int IdPatch { get; private set; }

        public int IdGameMode { get; private set; }

        public int IdGameEvent { get; private set; }

        public int IdSurvivor1 { get; private set; }

        public int IdSurvivor2 { get; private set; }

        public int IdSurvivor3 { get; private set; }

        public int IdSurvivor4 { get; private set; }

        public DateTime? DateTimeMatch { get; private set; }

        public string? GameTimeMatch { get; private set; }

        public int CountKills { get; private set; }

        public int CountHooks { get; private set; }

        public int NumberRecentGenerators { get; private set; }

        public string? DescriptionGame { get; private set; }

        public byte[]? ResultMatch { get; private set; }

        public int IdMatchAttribute { get; private set; }

        public static (GameStatisticDomain? GameStatisticDomain, string? Message) Create(
            int idGameStatistic, int idKiller, int idMap, int idWhoPlacedMap, int idWhoPlacedMapWin, int idPatch, int idGameMode, int idGameEvent, int idSurvivors1, int idSurvivors2, int idSurvivors3, int idSurvivors4,
            DateTime? dateTimeMatch, string? gameTimeMatch, int countKills, int countHooks, int numberRecentGenerators, string? descriptionGame, byte[]? resultMatch, int idMatchAttribute)
        {
            string message = string.Empty;
            DateTime MinAllowedDate = new DateTime(2016, 6, 14);
            const int MaxKills = 4;
            const int MinKills = 0;
            const int MaxHooks = 12;
            const int MinHooks = 0;
            const int MaxRecentGenerators = 5;
            const int MinRecentGenerators = 0;
            const int MaxDescriptionGameLength = 2000;


            if (idKiller < 0)
            {
                return (null, "Вы не указали ID Убийцы.");
            }

            if (idMap < 0 && idWhoPlacedMap < 0 && idWhoPlacedMapWin < 0)
            {
                return (null, "Вы не указали ID карты, либо кто ее поставил\\чья карта выпала.");
            }

            if (idPatch < 0)
            {
                return (null, "Вы не указали ID патча.");
            }

            if (idGameMode < 0 && idGameEvent < 0)
            {
                return (null, "Вы не указали ID режима или события.");
            }

            if (idSurvivors1 < 0 && idSurvivors2 < 0 && idSurvivors3 < 0 && idSurvivors4 < 0)
            {
                return (null, "Вы не указали ID одного из выживших.");
            }

            if (idMatchAttribute < 0)
            {
                return (null, $"Вы не выбрали атрибут для матча.");
            }

            if (dateTimeMatch < MinAllowedDate)
            {
                return (null, $"Минимально допустимая дата может быть {MinAllowedDate}");
            } 

            if (string.IsNullOrWhiteSpace(gameTimeMatch)) // TODO : Добавить проверки к времени игры.
            {

            }

            if (countKills > MaxKills | countKills < MinKills)
            {
                return (null, $"Количество убийств не может быть больше {MaxKills} и меньше {MinKills}");
            }

            if (countHooks > MaxHooks | countHooks < MinHooks)
            {
                return (null, $"Количество повесов не может быть больше {MaxHooks} и меньше {MinHooks}");
            }

            if (numberRecentGenerators > MaxRecentGenerators | MaxRecentGenerators < MinRecentGenerators)
            {
                return (null, $"Количество оставшихся генераторов не может быть больше {MaxRecentGenerators} и меньше {MinRecentGenerators}");
            }
            
            if (descriptionGame?.Length > MaxDescriptionGameLength)
            {
                return (null, $"Длинна описание превышает {MaxDescriptionGameLength}");
            }

            var gameStat = new GameStatisticDomain(
                idGameStatistic, idKiller, idMap, idWhoPlacedMap, idWhoPlacedMapWin, idPatch, idGameMode, idGameEvent, idSurvivors1, idSurvivors2, idSurvivors3, idSurvivors4,
                dateTimeMatch, gameTimeMatch, countKills, countHooks, numberRecentGenerators, descriptionGame, resultMatch: resultMatch, idMatchAttribute: idMatchAttribute);

            return (gameStat, message);
        }
    }
}