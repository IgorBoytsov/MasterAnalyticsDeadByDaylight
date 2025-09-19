using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class KillerPerk : Entity<Guid>
    {
        public int OldId { get; private set; }
        public Guid KillerId { get; private set; }
        public KillerPerkName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }
        public KillerPerkCategoryId? CategoryId { get; private set; }

        public Killer Killer { get; private set; } = null!;

        private KillerPerk() { }

        private KillerPerk(Guid id, Guid killerId, int oldId, KillerPerkName name, ImageKey? image) : base(id)
        {
            OldId = oldId;
            KillerId = killerId;
            Name = name;
            ImageKey = image;
        }

        public static KillerPerk Create(Guid killerId, int oldId, string name, ImageKey? image, int? categoryId)
        {
            var nameVo = KillerPerkName.Create(name);

            var killerPerk = new KillerPerk(Guid.NewGuid(), killerId, oldId, nameVo, image);

            if (categoryId is not null)
                killerPerk.CategoryId = KillerPerkCategoryId.From(categoryId.Value);

            return killerPerk;
        }

        public void AssignCategory(KillerPerkCategoryId categoryId) => this.CategoryId = categoryId;

        internal void UpdateName(KillerPerkName killerPerkName)
        {
            if (Name != killerPerkName)
                Name = killerPerkName;
        }

        internal void UpdateImageKey(ImageKey? newImageKey)
        {
            if (ImageKey != newImageKey)
                ImageKey = newImageKey;
        }
    }
}