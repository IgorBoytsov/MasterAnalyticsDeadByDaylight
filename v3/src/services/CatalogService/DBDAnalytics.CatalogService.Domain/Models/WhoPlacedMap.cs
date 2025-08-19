using DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class WhoPlacedMap : AggregateRoot<WhoPlacedMapId>
    {
        public int OldId { get; private set; }
        public PlacedMapName Name { get; private set; } = null!;

        private WhoPlacedMap() { }

        private WhoPlacedMap(int oldId, PlacedMapName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static WhoPlacedMap Create(int oldId, string name)
        {
            var nameVo = PlacedMapName.Create(name);

            return new WhoPlacedMap(oldId, nameVo);
        }
    }
}