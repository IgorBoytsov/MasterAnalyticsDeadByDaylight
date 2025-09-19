using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Role : AggregateRoot<RoleId>
    {
        public int OldId { get; private set; }
        public RoleName Name { get; private set; } = null!;

        private Role() { }

        private Role(int oldId, RoleName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static Role Create(int oldId, string name)
        {
            var nameVo = RoleName.Create(name);

            return new Role(oldId, nameVo);
        }

        public void UpdateName(RoleName roleName)
        {
            if(Name != roleName)
                Name = roleName;
        }
    }
}