using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class Match : AggregateRoot<MatchId>
    {
        public UserId UserId { get; private set; }
        public GameModeId GameModeId { get; private set; }
        public GameEventId GameEventId { get; private set; }
        public MapId MapId { get; private set; }
        public WhoPlacedMapId WhoPlacedMapId { get; private set; }
        public WhoPlacedMapId WhoPlacedMapWinId {  get; private set; }
        public PatchId PatchId { get; private set; }
        public CountKill CountKills { get; private set; }
        public CountHook CountHooks { get; private set; }
        public RecentGenerator CountRecentGenerators { get; private set; }
        public MatchStartedAt StartedAt { get; private set; }
        public MatchDuration Duration { get; private set; }

        private readonly List<KillerPerformance> _killerPerformances = [];
        public IReadOnlyCollection<KillerPerformance> KillerPerformances => _killerPerformances.AsReadOnly();

        private readonly List<SurvivorPerformance> _survivorPerformances = [];
        public IReadOnlyCollection<SurvivorPerformance> SurvivorPerformances => _survivorPerformances.AsReadOnly();

        private Match() { }

        private Match(
            MatchId id, UserId userId,
            GameModeId gameModeId, GameEventId gameEventId,
            MapId mapId, WhoPlacedMapId whoPlacedMapId, WhoPlacedMapId whoPlacedMapWinId,
            PatchId patchId,
            CountKill countKills, CountHook countHooks, RecentGenerator countRecentGenerators,
            MatchStartedAt startedAt, MatchDuration duration) : base(id)
        {
            UserId = userId;
            GameModeId = gameModeId;
            GameEventId = gameEventId;
            MapId = mapId;
            WhoPlacedMapId = whoPlacedMapId;
            WhoPlacedMapWinId = whoPlacedMapWinId;
            PatchId = patchId;
            CountKills = countKills;
            CountHooks = countHooks;
            CountRecentGenerators = countRecentGenerators;
            StartedAt = startedAt;
            Duration = duration;
        }

        public static Match Create(
            Guid userId,
            int gameModeId, int gameEventId,
            Guid mapId, int whoPlacedMapId, int whoPlacedMapWinId,
            int patchId,
            int countKills, int countHooks, int countRecentGenerators,
            DateTime startedAt, TimeSpan duration)
        {
            var matchId = MatchId.CreateNew();
            var userIdVO = UserId.From(userId);

            var gameModeIdVo = GameModeId.From(gameModeId);
            var gameEventIdVo = GameEventId.From(gameEventId);
            var mapIdVo = MapId.From(mapId);
            var whoPlacedMapIdVo = WhoPlacedMapId.From(whoPlacedMapId);
            var whoPlacedMapWinIdVo = WhoPlacedMapId.From(whoPlacedMapWinId);
            var countKillsVo = CountKill.From(countKills);
            var countHooksVo = CountHook.From(countHooks);
            var countRecentGeneratorsVo = RecentGenerator.From(countRecentGenerators);
            var patchIdVo = PatchId.From(patchId);
            var startedAtVo = MatchStartedAt.Create(startedAt);
            var durationVo = MatchDuration.Create(duration);

            return new Match(
                matchId, userIdVO,
                gameModeIdVo, gameEventIdVo,
                mapIdVo, whoPlacedMapIdVo, whoPlacedMapWinIdVo,
                patchIdVo,
                countKillsVo, countHooksVo, countRecentGeneratorsVo,
                startedAtVo, durationVo);
        }

        public void AddKillerPerformance(KillerPerformance killerPerformance)
        {
            if (_killerPerformances.Any(kp => kp.KillerId == killerPerformance.KillerId))
                throw new Exception("В одном матче не может быть идентичных киллеров.");

            _killerPerformances.Add(killerPerformance);
        }

        public void AddSurvivorPerformance(SurvivorPerformance survivorPerformance) => _survivorPerformances.Add(survivorPerformance);
    }
}