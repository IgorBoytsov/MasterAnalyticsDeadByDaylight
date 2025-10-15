using DBDAnalytics.CatalogService.Domain.ValueObjects.Platform;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Platform : AggregateRoot<PlatformId>
    {
        public int OldId { get; private set; }
        public PlatformName Name { get; private set; } = null!;

        private Platform(int oldId, PlatformName name)
        {
            OldId = oldId;
            Name = name;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Platform Create(int oldId, string name)
        {
            var nameVo = PlatformName.Create(name);

            return new Platform(oldId, nameVo);
        }

        public void UpdateName(PlatformName platformName)
        {
            if(Name != platformName)
                Name = platformName;
        }
    }
}