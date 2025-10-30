using DBDAnalytics.MatchService.Domain.ValueObjects.KillerPerformance;
using DBDAnalytics.Shared.Domain.ValueObjects;
using DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.MatchService.Domain.Models
{
    public sealed class KillerPerformance : Entity<KillerPerformanceId>
    {
        public MatchId MatchId { get; private set; }
        public KillerId KillerId { get; private set; }
        public OfferingId OfferingId { get; private set; }
        public PlayerAssociationId AssociationId { get; private set; }
        public PlatformId PlatformId { get; private set; }
        public Prestige Prestige { get; private set; }
        public Score Score { get; private set; }
        public Experience Experience { get; private set; }
        public Blood NumberBloodDrops { get; private set; }
        public bool IsBot { get; private set; }
        public bool IsAnonymousMode { get; private set; }

        private readonly List<KillerPerk> _perks = [];
        public IReadOnlyCollection<KillerPerk> Perks => _perks.AsReadOnly();

        private readonly List<KillerAddon> _addons = [];
        public IReadOnlyCollection<KillerAddon> Addons => _addons.AsReadOnly();

        private KillerPerformance() { }

        private KillerPerformance(
            KillerPerformanceId id, MatchId matchId, KillerId killerId,
            OfferingId offeringId,
            PlayerAssociationId playerAssociationId, PlatformId platformId,
            Prestige prestige, Score score, Experience experience, Blood numberBloodDrops,
            bool isBot, bool isAnonymousMode) : base(id)
        {
            MatchId = matchId;
            KillerId = killerId;
            OfferingId = offeringId;
            AssociationId = playerAssociationId;
            PlatformId = platformId;
            Prestige = prestige;
            IsBot = isBot;
            IsAnonymousMode = isAnonymousMode;
            Score = score;
            Experience = experience;
            NumberBloodDrops = numberBloodDrops;
        }

        public static KillerPerformance Create(
            MatchId matchId, Guid killerId,
            Guid offeringId,
            int playerAssociationId, int platformId,
            int prestige, int score, int experience, int numberBloodDrops,
            bool isBot, bool isAnonymousMode)
        {
            var killerPerformanceIdVo = KillerPerformanceId.CreateNew();

            var killerIdVo = KillerId.From(killerId);
            var offeringIdVo = OfferingId.From(offeringId);
            var associationVo = PlayerAssociationId.Form(playerAssociationId);
            var platformVo = PlatformId.From(platformId);
            var prestigeVo = Prestige.From(prestige);
            var scoreVo = Score.From(score);
            var experienceVo = Experience.From(experience);
            var numberBloodDropsVo = Blood.From(numberBloodDrops);

            return new KillerPerformance(
                killerPerformanceIdVo, matchId, killerIdVo, 
                offeringIdVo, 
                associationVo, platformVo,
                prestigeVo, scoreVo, experienceVo, numberBloodDropsVo,
                isBot, isAnonymousMode);
        }

        public void AddPerk(Guid perkId)
        {
            if (_perks.Any(p => p.PerkId == perkId))
                throw new Exception("Нельзя дублировать одинаковые перки у одного киллера.");

            var newPerk = KillerPerk.Create(this.Id, perkId);
            _perks.Add(newPerk);
        }

        public void AddAddon(Guid addonId)
        {
            if (_addons.Any(p => p.AddonId == addonId))
                throw new Exception("Нельзя дублировать одинаковые улучшения у одного киллера.");

            var newAddon = KillerAddon.Create(this.Id, addonId);
            _addons.Add(newAddon);
        }
    }
}