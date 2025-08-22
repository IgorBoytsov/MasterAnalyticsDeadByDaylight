using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Rarity : AggregateRoot<RarityId>
    {
        public int OldId { get; private set; }
        public RarityName Name { get; private set; } = null!;

        private readonly List<Offering> _offerings = [];
        public IReadOnlyCollection<Offering> Offerings => _offerings.AsReadOnly();

        private Rarity() { }

        private Rarity(int oldId, RarityName rarityName)
        {
            OldId = oldId;
            Name = rarityName;
        }

        public static Rarity Create(int oldId, string name)
        {
            var nameVo = RarityName.Create(name);

            return new Rarity(oldId, nameVo);
        }
    }
}