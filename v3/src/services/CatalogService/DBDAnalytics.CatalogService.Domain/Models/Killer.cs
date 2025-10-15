using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Killer;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Primitives;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Killer : AggregateRoot<Guid>
    {
        public int OldId { get; private set; }
        public KillerName Name { get; private set; } = null!;
        public ImageKey? KillerImageKey { get; private set; }
        public ImageKey? AbilityImageKey { get; private set; }

        private readonly List<KillerPerk> _killerPerks = [];
        public IReadOnlyCollection<KillerPerk> KillerPerks => _killerPerks.AsReadOnly();

        private readonly List<KillerAddon> _killerAddons = [];
        public IReadOnlyCollection<KillerAddon> KillerAddons => _killerAddons.AsReadOnly();

        private Killer() { }

        private Killer(Guid id, int oldId, KillerName name, ImageKey? killerImageKey, ImageKey? abilityImageKey) : base(id)
        {
            OldId = oldId;
            Name = name;
            KillerImageKey = killerImageKey;
            AbilityImageKey = abilityImageKey;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Killer Create(int oldId, string name, ImageKey? killerImageKey, ImageKey? abilityImageKey)
        {
            var nameVo = KillerName.Create(name);
            return new Killer(Guid.NewGuid(), oldId, nameVo, killerImageKey, abilityImageKey);
        }

        public void UpdateName(KillerName killerName)
        {
            if (Name != killerName)
                Name = killerName;
        }

        public void UpdateImagePortrait(ImageKey? newImagePortraitKey)
        {
            if (KillerImageKey != newImagePortraitKey)
                KillerImageKey = newImagePortraitKey;
        }

        public void UpdateImageAbility(ImageKey? newImageAbilityKey)
        {
            if (AbilityImageKey != newImageAbilityKey)
                AbilityImageKey = newImageAbilityKey;
        }

        #region Улучшения

        /// <exception cref="DuplicateException"></exception>
        public KillerAddon AddAddon(int oldId, string addonName, ImageKey? imageKey)
        {
            GuardException.Against.That(_killerAddons.Any(a => a.Name.Value == addonName), () => new DuplicateException($"Улучшение {addonName} уже существует у киллера."));

            var newAddon = KillerAddon.Create(oldId, addonName, imageKey, this.Id);

            _killerAddons.Add(newAddon);

            return newAddon;
        }

        /// <exception cref="NotFoundException"></exception>
        public bool RemoveAddon(Guid addonId)
        {
            var addonToRemove = _killerAddons.FirstOrDefault(a => a.Id == addonId);

            GuardException.Against.That(addonToRemove is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Улучшение c ID {addonId} не существует у {this.Name}.")));
            
            _killerAddons.Remove(addonToRemove!);

            return true;
        }

        public void ClearAddons() => _killerAddons.Clear();

        #endregion

        #region Перки

        /// <exception cref="DuplicateException"></exception>
        public KillerPerk AddPerk(int oldId, string name, ImageKey? image, int? categoryId)
        {
            GuardException.Against.That(_killerPerks.Any(p => p.Name.Value == name), () => new DuplicateException($"Перк {name} уже существует у киллера."));

            var newPerk = KillerPerk.Create(this.Id, oldId, name, image, categoryId);

            _killerPerks.Add(newPerk);

            return newPerk;
        }

        /// <exception cref="NotFoundException"></exception>
        public bool RemovePerk(Guid perkId)
        {
            var perkToRemove = _killerPerks.FirstOrDefault(p => p.Id == perkId);

            GuardException.Against.That(perkToRemove is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Перк c ID {perkId} не существует у {this.Name}.")));

            _killerPerks.Remove(perkToRemove!);

            return true;
        }

        public void ClearPerks() => _killerPerks.Clear();

        /// <exception cref="NotFoundException"></exception>
        public void AssignCategoryToPerk(Guid perkId, KillerPerkCategoryId categoryId)
        {
            var perk = _killerPerks.FirstOrDefault(p => p.Id == perkId);

            GuardException.Against.That(perk is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Перк c ID {perkId} не существует у {this.Name}.")));

            perk!.AssignCategory(categoryId);
        }

        /// <exception cref="NotFoundException"></exception>
        public void UpdateAddon(Guid addonId, KillerAddonName killerAddonName, ImageKey? newImageKey)
        {
            var killerAddon = _killerAddons.FirstOrDefault(ka => ka.Id == addonId);

            GuardException.Against.That(killerAddon is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Улучшение с id {addonId} не было найдено у киллера {this.Id}.")));

            killerAddon!.UpdateName(killerAddonName);
            killerAddon.UpdateImageKey(newImageKey);
        }

        /// <exception cref="NotFoundException"></exception>
        public void UpdatePerk(Guid perkId, KillerPerkName killerPerkName, ImageKey? newImageKey)
        {
            var killerPerk = _killerPerks.FirstOrDefault(ka => ka.Id == perkId);

            GuardException.Against.That(killerPerk is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Перк с id {perkId} не был найден у киллера {this.Id}.")));

            killerPerk!.UpdateName(killerPerkName);
            killerPerk.UpdateImageKey(newImageKey);
        }

        #endregion
    }
}