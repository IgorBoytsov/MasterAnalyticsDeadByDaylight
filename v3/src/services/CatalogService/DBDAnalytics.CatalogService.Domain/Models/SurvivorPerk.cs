using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerk;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class SurvivorPerk : Entity<Guid>
    {
        public int OldId { get; private set; }
        public SurvivorPerkName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }
        public Guid SurvivorId { get; private set; }
        public SurvivorPerkCategoryId? CategoryId { get; private set; }

        public Survivor Survivor { get; private set; } = null!;

        private SurvivorPerk() { }

        private SurvivorPerk(Guid id, Guid survivorId, int oldId, SurvivorPerkName name, ImageKey? imageKey) : base(id)
        {
            OldId = oldId;
            Name = name;
            ImageKey = imageKey;
            SurvivorId = survivorId;
        }

        public static SurvivorPerk Create(Guid survivorId, string name, int oldId, ImageKey? imageKey, int? categoryId)
        {
            var nameVo = SurvivorPerkName.Create(name);

            var nuwPerk = new SurvivorPerk(Guid.NewGuid(), survivorId, oldId, nameVo, imageKey);

            if (categoryId.HasValue)
                nuwPerk.CategoryId = SurvivorPerkCategoryId.From(categoryId.Value);

            return nuwPerk;
        }

        internal void AssignCategory(SurvivorPerkCategoryId categoryId) => CategoryId = categoryId;

        internal void RemoveCategory() => CategoryId = null;
    }
}