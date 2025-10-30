using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;
using DBDAnalytics.MatchService.Domain.ValueObjects.SurvivorPerformance;
using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class SurvivorPerformance : Entity<SurvivorPerformanceId>
    {
        public MatchId MatchId { get; private set; }
        public SurvivorId SurvivorId { get; private set; }
        public OfferingId OfferingId { get; private set; }
        public PlayerAssociationId AssociationId { get; private set; }
        public TypeDeathId TypeDeathId { get; private set; }
        public PlatformId PlatformId { get; private set; }
        public Prestige Prestige { get; private set; }
        public Score Score { get; private set; }
        public Experience Experience { get; private set; }
        public Blood NumberBloodDrops { get; private set; }
        public bool IsBot { get; private set; }
        public bool IsAnonymousMode { get; private set; }

        private readonly List<SurvivorItem> _items = [];
        public IReadOnlyCollection<SurvivorItem> Items => _items.AsReadOnly();

        private readonly List<SurvivorPerk> _perks = [];
        public IReadOnlyCollection<SurvivorPerk> Perks => _perks.AsReadOnly();

        private SurvivorPerformance() { }

        private SurvivorPerformance(
            SurvivorPerformanceId id, MatchId matchId, SurvivorId survivorId,
            OfferingId offeringId,
            PlayerAssociationId playerAssociationId, TypeDeathId typeDeathId, PlatformId platformId,
            Prestige prestige, Score score, Experience experience, Blood numberBloodDrops,
            bool isBot, bool isAnonymousMode) : base(id)
        {
            MatchId = matchId;
            SurvivorId = survivorId;
            OfferingId = offeringId;
            AssociationId = playerAssociationId;
            TypeDeathId = typeDeathId;
            PlatformId = platformId;
            Prestige = prestige;
            IsBot = isBot;
            IsAnonymousMode = isAnonymousMode;
            Score = score;
            Experience = experience;
            NumberBloodDrops = numberBloodDrops;
        }

        public static SurvivorPerformance Create(
            MatchId matchId, Guid survivorId,
            Guid offeringId,
            int playerAssociationId, int typeDeathId, int platformId,
            int prestige, int score, int experience, int numberBloodDrops,
            bool isBot, bool isAnonymousMode)
        {
            var survivorPerformanceIdVo = SurvivorPerformanceId.CreateNew();

            var survivorIdVo = SurvivorId.From(survivorId);
            var offeringIdVo = OfferingId.From(offeringId);
            var associationVo = PlayerAssociationId.Form(playerAssociationId);
            var typeDeathIdVo = TypeDeathId.From(typeDeathId);
            var platformVo = PlatformId.From(platformId);
            var prestigeVo = Prestige.From(prestige);
            var scoreVo = Score.From(score);
            var experienceVo = Experience.From(experience);
            var numberBloodDropsVo = Blood.From(numberBloodDrops);

            return new SurvivorPerformance(
                survivorPerformanceIdVo, matchId, survivorIdVo,
                offeringIdVo, 
                associationVo, typeDeathIdVo, platformVo, 
                prestigeVo, scoreVo, experienceVo, numberBloodDropsVo,
                isBot, isAnonymousMode);
        }

        public void AddItem(Guid itemId, List<Guid> addonIds)
        {
            var newItem = SurvivorItem.Create(this.Id, itemId);

            foreach (var addonId in addonIds)
                newItem.AddAddon(addonId);

            _items.Add(newItem);
        }

        public void AddPerk(Guid perkId)
        {
            if (_perks.Any(p => p.PerkId == perkId))
                throw new Exception("Нельзя дублировать одинаковые перки у одного выжившего.");

            var newPerk = SurvivorPerk.Create(this.Id, perkId);
            _perks.Add(newPerk);
        }
    }
}