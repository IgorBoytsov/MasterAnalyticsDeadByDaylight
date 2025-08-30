using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Item;
using DBDAnalytics.CatalogService.Domain.ValueObjects.ItemAddon;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Primitives;
using DBDAnalytics.Shared.Domain.Results;

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

        public void UpdateName(ItemName newItemName)
        {
            if (Name != newItemName)
                Name = newItemName;
        }

        public void UpdateImageKey(ImageKey? newImageKey) => ImageKey = newImageKey;

        public ItemAddon AddAddon(int oldId, string name, ImageKey? imageKey, int? rarityId)
        {
            GuardException.Against.That(_itemAddons.Any(p => p.Name.Value == name), () => new DuplicateException($"Улучшение {name} уже существует у предмета."));

            var itemAddon = ItemAddon.Create(this.Id, oldId, name, imageKey, rarityId);
            _itemAddons.Add(itemAddon);
            return itemAddon;
        }

        public void UpdateItemAddon(Guid addonId, string name, ImageKey? imageKey)
        {
            var itemAddon = _itemAddons.FirstOrDefault(ia => ia.Id == addonId) 
                ?? throw new DomainException(new Error(ErrorCode.NotFound, $"Улучшение с id {addonId} не было найдено у предмета {this.Id}."));

            itemAddon.UpdateName(ItemAddonName.Create(name));
            itemAddon.UpdateImageKey(imageKey);
        }

        public bool RemoveAddon(Guid idItem)
        {
            var itemAddonToDelete = _itemAddons.FirstOrDefault(ia => ia.Id == idItem);

            if (itemAddonToDelete is null)
                return false;

            _itemAddons.Remove(itemAddonToDelete);

            return true;
        }

        public void AssignRarity(Guid itemAddonId, RarityId rarityId)
        {
            var itemAddon = _itemAddons.FirstOrDefault(p => p.Id == itemAddonId);

            GuardException.Against.That(itemAddon is null, () => new InvalidOperationException("Аддон не найден."));

            itemAddon!.AssignCategory(rarityId);
        }
    }
}