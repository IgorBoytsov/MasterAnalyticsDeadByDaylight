using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerAddon;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class KillerAddon : Entity<Guid>
    {
        public int OldId { get; private set; }
        public KillerAddonName Name { get; private set; } = null!;
        public ImageKey? ImageKey { get; private set; }
        public Guid KillerId { get; private set; }

        public Killer Killer { get; private set; } = null!;

        private KillerAddon() { }

        private KillerAddon(Guid id, int oldId, KillerAddonName name, ImageKey? imageKey, Guid killerId) : base(id)
        {
            OldId = oldId;
            Name = name;
            ImageKey = imageKey;
            KillerId = killerId;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static KillerAddon Create(int oldId, string name, ImageKey? imageKey, Guid killerId)
        {
            var nameVo = KillerAddonName.Create(name);

            return new KillerAddon(Guid.NewGuid(), oldId, nameVo, imageKey, killerId);
        }

        internal void UpdateName(KillerAddonName killerAddonName)
        {
            if(Name != killerAddonName)
                Name = killerAddonName;
        }

        internal void UpdateImageKey(ImageKey? newImageKey)
        {
            if(ImageKey != newImageKey)
                ImageKey = newImageKey;
        }
    }
}