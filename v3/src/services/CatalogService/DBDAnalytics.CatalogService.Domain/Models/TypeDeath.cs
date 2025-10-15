using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class TypeDeath : AggregateRoot<TypeDeathId>
    {
        public int OldId { get; private set; }
        public TypeDeathName Name { get; private set; } = null!;

        private TypeDeath(int oldId, TypeDeathName name)
        {
            OldId = oldId;
            Name = name;
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static TypeDeath Create(int oldId, string name)
        {
            var nameVo = TypeDeathName.Create(name);

            return new TypeDeath(oldId, nameVo);
        }

        public void UpdateName(TypeDeathName typeDeathName)
        {
            if(Name != typeDeathName)
                Name = typeDeathName;
        }
    }
}