using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Map : Entity<Guid>
    {
        public int OldId { get; private set; }
        public MapName Name { get; private set; } = null!;
        public ImageKey? ImageKey {  get; private set; }
        public Guid MeasurementId { get; private set; }

        public Measurement Measurement { get; private set; } = null!;

        private Map() { }

        private Map(Guid id, int oldId, MapName name, ImageKey? image, Guid measurementId) : base(id)
        {
            OldId = oldId;
            Name = name;
            ImageKey = image;
            MeasurementId = measurementId;
        }

        public static Map Create(int oldId, string name, ImageKey? image, Guid measurementId)
        {
            var nameVo = MapName.Create(name);

            return new Map(Guid.NewGuid(), oldId, nameVo, image, measurementId);
        }

        internal void UpdateName(MapName mapName)
        {
            if(Name != mapName)
                Name = mapName;
        }

        internal void UpdateImageKey(ImageKey? newImageKey)
        {
            if(ImageKey != newImageKey)
                ImageKey = newImageKey;
        }
    }
}