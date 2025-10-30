using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared
{
    public readonly record struct Blood
    {
        public const int MAX = 2;
        public const int MIN = 0;

        public int Value { get; }

        internal Blood(int value) => Value = value;

        public static Blood From(int value)
        {
            GuardException.Against.That(value > MAX || value < MIN, () => new AggregateException($"Кровавых капель может быть в диапазоне от {MIN} до {MAX}"));

            return new Blood(value);
        }

        public static implicit operator int(Blood blood) => blood.Value;
    }
}