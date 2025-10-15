using DBDAnalytics.CatalogService.Domain.ValueObjects.SurvivorPerkCategory;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class SurvivorPerkCategory : AggregateRoot<SurvivorPerkCategoryId>
    {
        public int OldId { get; private set; }
        public SurvivorPerkCategoryName Name { get; private set; } = null!;

        private SurvivorPerkCategory() { }

        private SurvivorPerkCategory(int oldId, SurvivorPerkCategoryName name)
        {
            OldId = oldId;
            Name = name;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static SurvivorPerkCategory Create(int oldId, string name)
        {
            var nameVo = SurvivorPerkCategoryName.Create(name);

            return new SurvivorPerkCategory(oldId, nameVo);
        }

        public void UpdateName(SurvivorPerkCategoryName survivorPerkCategoryName)
        {
            if(Name != survivorPerkCategoryName)
                Name = survivorPerkCategoryName;
        }
    }
}