using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Item : AggregateRoot<Guid>
    {
        public int OldId { get; private set; }
        public ItemName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }

        private readonly List<ItemAddon> _itemAddons = [];
        public IReadOnlyCollection<ItemAddon> ItemAddons => _itemAddons.AsReadOnly();

        private Item() { }

        private Item(Guid id, int oldId, ItemName name, ImageKey? imageKey) : base(id) 
        {
            OldId = oldId;
            Name = name;
            ImageKey = imageKey;
        }

        public static Item Create(int oldId, string name, ImageKey? imageKey)
        {
            var nameVo = ItemName.Create(name);

            return new Item(Guid.NewGuid(), oldId, nameVo, imageKey);
        }

        public ItemAddon AddAddon(int oldId, string name, ImageKey? imageKey, int? rarityId)
        {
            GuardException.Against.That(_itemAddons.Any(p => p.Name.Value == name), () => new DuplicateException($"Улучшение {name} уже существует у предмета."));

            var itemAddon = ItemAddon.Create(this.Id, oldId, name, imageKey, rarityId);
            _itemAddons.Add(itemAddon);
            return itemAddon;
        }

        public void AssignRarity(Guid itemAddonId, RarityId rarityId)
        {
            var itemAddon = _itemAddons.FirstOrDefault(p => p.Id == itemAddonId);

            GuardException.Against.That(itemAddon is null, () => new InvalidOperationException("Аддон не найден."));

            itemAddon!.AssignCategory(rarityId);
        }
    }
}