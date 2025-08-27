using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Survivor;
using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Survivor : AggregateRoot<Guid>
    {
        public int OldId { get; private set; }
        public SurvivorName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }

        private readonly List<SurvivorPerk> _survivorPerks = [];
        public IReadOnlyCollection<SurvivorPerk> SurvivorPerks => _survivorPerks.AsReadOnly();

        private Survivor() { }

        private Survivor(Guid id, int oldId, SurvivorName name, ImageKey? imageKey) : base(id)
        {
            OldId = oldId;
            Name = name;
            ImageKey = imageKey;
        }

        public static Survivor Create(int oldId, string name, ImageKey? imageKey)
        {
            var nameVo = SurvivorName.Create(name);

            return new Survivor(Guid.NewGuid(), oldId, nameVo, imageKey);
        }

        public SurvivorPerk AddPerk(string name, int oldId, ImageKey? imageKey, int? categoryId)
        {
            GuardException.Against.That(_survivorPerks.Any(p => p.Name.Value == name), () => new DuplicateException($"Перк {name} уже существует у выжившего."));

            var newPerk = SurvivorPerk.Create(this.Id, name, oldId, imageKey, categoryId);

            _survivorPerks.Add(newPerk);

            return newPerk;
        }

        public bool RemovePerk(Guid perkId)
        {
            var perkToRemove = _survivorPerks.FirstOrDefault(p => p.Id == perkId);

            if (perkToRemove is null)
                return false;
            
            _survivorPerks.Remove(perkToRemove);

            return true;
        }

        public void ClearPerks() => _survivorPerks.Clear();

        public void AssignCategoryToPerk(SurvivorPerkCategoryId categoryId, Guid perkId)
        {
            var perk = _survivorPerks.FirstOrDefault(p => p.Id == perkId);

            GuardException.Against.That(perk is null, () => new InvalidOperationException("Перк не найден."));

            perk!.AssignCategory(categoryId);
        }

        public void RemoveCategoryFromPerk(Guid perkId)
        {
            var perk = _survivorPerks.FirstOrDefault(p => p.Id == perkId);
            
            GuardException.Against.That(perk is null, () => new InvalidOperationException("Перк не найден."));

            perk!.RemoveCategory();
        }
    }
}