using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.WhoPlacedMap
{
    public readonly struct WhoPlacedMapId
    {
        public int Value { get; }

        internal WhoPlacedMapId(int value) => Value = value;

        public static WhoPlacedMapId From(int value)
        {
            GuardException.Against.That(value <= 0, () => new ArgumentException("Id того, кто поставил карту должен быть положительным числом.", nameof(value)));

            return new WhoPlacedMapId(value);
        }

        public static implicit operator int(WhoPlacedMapId whoPlacedMapId) => whoPlacedMapId.Value;
    }
}