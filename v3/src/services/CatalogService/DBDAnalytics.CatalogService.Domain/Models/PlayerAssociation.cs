using DBDAnalytics.CatalogService.Domain.ValueObjects.PlayerAssociation;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class PlayerAssociation : AggregateRoot<PlayerAssociationId>
    {
        public int OldId { get; private set; }
        public PlayerAssociationName Name { get; private set; } = null!;

        private PlayerAssociation() { }

        private PlayerAssociation(int oldId, PlayerAssociationName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static PlayerAssociation Create(int oldId, string name)
        {
            var nameVo = PlayerAssociationName.Create(name);

            return new PlayerAssociation(oldId, nameVo);    
        }

        public void UpdateName(PlayerAssociationName playerAssociationName)
        {
            if(Name != playerAssociationName)
                Name = playerAssociationName;
        }
    }
}