using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class OfferingCategory : Entity<OfferingCategoryId>
    {
        public int OldId { get; private set; }
        public OfferingCategoryName Name { get; private set; } = null!;

        private readonly List<Offering> _offerings = [];
        public IReadOnlyCollection<Offering> Offerings => _offerings.AsReadOnly();

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
    }
}