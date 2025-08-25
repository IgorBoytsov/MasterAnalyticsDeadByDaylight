using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Offering;
using DBDAnalytics.CatalogService.Domain.ValueObjects.OfferingCategory;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Rarity;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Offering : AggregateRoot<Guid>
    {
        public int OldId { get; private set; }
        public OfferingName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; } 
        public RoleId RoleId { get; private set; }
        public RarityId? RarityId { get; private set; }
        public OfferingCategoryId? CategoryId { get; private set; }

        private Offering() { }

        private Offering(Guid id, int oldId, OfferingName name, ImageKey? imageKey, RoleId roleId) : base(id)
        {
            OldId = oldId;
            Name = name;
            ImageKey = imageKey;
            RoleId = roleId;
        }

        public static Offering Create(int oldId, string offeringName, ImageKey? imageKey, int roleId, int? rarityId, int? offeringCategoryId)
        {
            var offeringNameVo = OfferingName.Create(offeringName);
            var roleIdVo = RoleId.Form(roleId);

            var offering = new Offering(Guid.NewGuid(), oldId,offeringNameVo, imageKey, roleIdVo);

            if (rarityId.HasValue)
                offering.RarityId = ValueObjects.Rarity.RarityId.From(rarityId.Value);

            if(offeringCategoryId.HasValue)
                offering.CategoryId = ValueObjects.OfferingCategory.OfferingCategoryId.From(offeringCategoryId.Value);

            return offering;
        }

        ///// <exception cref="InvalidKillerPropertyException"></exception>
        public void AssignCategory(OfferingCategory category)
        {
            GuardException.Against.Null(category, nameof(category));

            CategoryId = category.Id;
        }

        public void RemoveCategory() => CategoryId = null;

        /// <exception cref="InvalidKillerPropertyException"></exception>
        public void AssignRarity(Rarity rarity)
        {
            GuardException.Against.Null(rarity, nameof(rarity));
            RarityId = rarity.Id;
        }

        public void RemoveRarity() => RarityId = null;
    }
}