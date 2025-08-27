using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using DBDAnalytics.Shared.Domain.Primitives;

namespace DBDAnalytics.CatalogService.Domain.Models
{
    public sealed class Measurement : AggregateRoot<Guid>
    {
        public int OldId { get; private set; }
        public MeasurementName Name { get; private set; } = null!;

        private readonly List<Map> _maps = [];
        public IReadOnlyCollection<Map> Maps => _maps.AsReadOnly();

        private Measurement() { }

        private Measurement(Guid id, int oldId, MeasurementName name) : base(id)
        {
            OldId = oldId;  
            Name = name;
        }

        public static Measurement Create(int oldId, string name)
        {
            var nameVo = MeasurementName.Create(name);

            return new Measurement(Guid.NewGuid(), oldId, nameVo);
        }

        public Map AddMap(int oldId, string mapName, ImageKey? imageKey)
        {
            GuardException.Against.That(_maps.Any(m => m.Name.Value == mapName), () => new DuplicateException($"Карта {mapName} уже существует в этой измерение."));
        
            var newMap = Map.Create(oldId, mapName, imageKey, this.Id);

            _maps.Add(newMap);

            return newMap;
        }

        public bool RemoveMap(Guid mapId)
        {
            var map = _maps.FirstOrDefault(m => m.Id == mapId);

            if (map is null)
                return false;

            _maps.Remove(map);

            return true;
        }

        public void ClearMaps() => _maps.Clear();   
    }
}