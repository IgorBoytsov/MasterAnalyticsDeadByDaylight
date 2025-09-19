using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Rarity : AggregateRoot<RarityId>
    {
        public int OldId { get; private set; }
        public RarityName Name { get; private set; } = null!;

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

        public void UpdateName(RarityName rarityName)
        {
            if(Name != rarityName)
                Name = rarityName;
        }
    }
}