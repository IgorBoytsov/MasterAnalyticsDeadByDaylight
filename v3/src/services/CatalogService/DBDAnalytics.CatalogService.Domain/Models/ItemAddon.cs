using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class ItemAddon : Entity<Guid>
    {
        public int OldId { get; private set; }
        public Guid ItemId { get; private set; }
        public ItemAddonName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }
        public RarityId? RarityId { get; private set; }

        public Item Item { get; private set; } = null!;

        private ItemAddon() { }

        private ItemAddon(Guid id, Guid itemId, int oldId, ItemAddonName name, ImageKey? imageKey) : base(id)
        {
            OldId = oldId;
            ItemId = itemId;
            Name = name;
            ImageKey = imageKey;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IdentifierOutOfRangeException"></exception>
        internal static ItemAddon Create(Guid itemId, int oldId, string name, ImageKey? imageKey, int? rarityId)
        {
            var nameVo = ItemAddonName.Create(name);

            var itemAddon = new ItemAddon(Guid.NewGuid(), itemId, oldId, nameVo, imageKey);

            if (rarityId.HasValue)
                itemAddon.RarityId = ValueObjects.Rarity.RarityId.From(rarityId.Value);

            return itemAddon;
        }

        public void AssignCategory(RarityId rarityId) => this.RarityId = rarityId;

        internal void UpdateName(ItemAddonName itemAddonName)
        {
            if(Name != itemAddonName)
                Name = itemAddonName;
        }

        internal void UpdateImageKey(ImageKey? imageKey)
        {
            if (ImageKey != imageKey)
                ImageKey = imageKey;
        }
    }
}