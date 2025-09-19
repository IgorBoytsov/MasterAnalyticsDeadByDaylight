using DBDAnalytics.CatalogService.Domain.ValueObjects.GameMode;
using DBDAnalytics.Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class GameMode : AggregateRoot<GameModeId>
    {
        public int OldId { get; private set; }
        public GameModeName Name { get; private set; } = null!;

        private GameMode() { }

        private GameMode(int oldId, GameModeName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static GameMode Create(int oldId, string name)
        {
            var nameVo = GameModeName.Create(name);

            return new GameMode(oldId, nameVo);
        }

        public void UpdateName(GameModeName gameModeName)
        {
            if(Name != gameModeName)
                Name = gameModeName;
        }
    }
}