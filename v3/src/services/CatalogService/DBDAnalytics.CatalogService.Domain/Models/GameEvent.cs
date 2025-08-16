using DBDAnalytics.CatalogService.Domain.ValueObjects.GameEvent;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class GameEvent : AggregateRoot<GameEventId>
    {
        public int OldId { get; private set; }
        public GameEventName Name { get; private set; } = null!;

        private GameEvent() { }

        private GameEvent(int oldId, GameEventName name)
        {
            OldId = oldId;
            Name = name;
        }

        public static GameEvent Create(int oldId, string name)
        {
            var nameVo = GameEventName.Create(name);

            return new GameEvent(oldId, nameVo);
        }
    }
}