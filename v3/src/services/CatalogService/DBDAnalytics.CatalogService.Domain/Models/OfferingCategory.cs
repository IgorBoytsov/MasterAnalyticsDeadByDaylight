using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class OfferingCategory : AggregateRoot<OfferingCategoryId>
    {
        public int OldId { get; private set; }
        public OfferingCategoryName Name { get; private set; } = null!;

        private OfferingCategory() { }

        private OfferingCategory(int oldId, OfferingCategoryName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static OfferingCategory Create(int oldId, string name)
        {
            var nameVo = OfferingCategoryName.Create(name);
            
            return new OfferingCategory(oldId, nameVo);
        }

        public void UpdateName(OfferingCategoryName offeringCategoryName)
        {
            if(Name != offeringCategoryName)
                Name = offeringCategoryName;
        }
    }
}