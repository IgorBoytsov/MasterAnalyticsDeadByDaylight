using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Primitives;
using Shared.Kernel.Results;

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

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Measurement Create(int oldId, string name)
        {
            var nameVo = MeasurementName.Create(name);

            return new Measurement(Guid.NewGuid(), oldId, nameVo);
        }

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="DuplicateException"></exception>
        public Map AddMap(int oldId, string mapName, ImageKey? imageKey)
        {
            GuardException.Against.That(_maps.Any(m => m.Name.Value == mapName), () => new DuplicateException($"Карта {mapName} уже существует в этой измерение."));
        
            var newMap = Map.Create(oldId, mapName, imageKey, this.Id);

            _maps.Add(newMap);

            return newMap;
        }

        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool RemoveMap(Guid mapId)
        {
            var map = _maps.FirstOrDefault(m => m.Id == mapId);

            GuardException.Against.That(map is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Карта c ID {mapId} не существует у измерения {this.Name}.")));

            _maps.Remove(map!);

            return true;
        }

        public void ClearMaps() => _maps.Clear();

        public void UpdateName(MeasurementName measurementName)
        {
            if(Name !=  measurementName)
                Name = measurementName;
        }

        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateMap(Guid mapId, MapName mapName, ImageKey? newImageKey)
        {
            var map = _maps.FirstOrDefault(m => m.Id == mapId);
            
            GuardException.Against.That(map is null, () => new NotFoundException(new Error(ErrorCode.NotFound, $"Карта с id {mapId} не была найдена у измерения {this.Name}.")));

            map!.UpdateName(mapName);
            map.UpdateImageKey(newImageKey);
        }
    }
}