using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class KillerPerkCategory : AggregateRoot<KillerPerkCategoryId>
    {
        public int OldId { get; private set; }
        public KillerPerkCategoryName Name { get; private set; } = null!;

        private KillerPerkCategory() { }

        private KillerPerkCategory(int oldId, KillerPerkCategoryName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static KillerPerkCategory Create(int oldId, string name)
        {
            var nameVo = KillerPerkCategoryName.Create(name);

            return new KillerPerkCategory(oldId, nameVo);
        }

        public void UpdateName(string newName) => Name = KillerPerkCategoryName.Create(newName);
    }
}