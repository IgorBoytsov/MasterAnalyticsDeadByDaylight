using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared
{
    public readonly record struct Prestige
    {
        public const int MAX = 100;
        public const int MIN = 0;

        public int Value { get; }

        internal Prestige(int value) => Value = value;

        public static Prestige From(int value)
        {
            GuardException.Against.That(value > MAX || value < MIN, () => new AggregateException($"Престиж может быть в диапазоне от {MIN} до {MAX}"));
        
            return new Prestige(value);
        }

        public static implicit operator int(Prestige prestige) => prestige.Value;
    }
}