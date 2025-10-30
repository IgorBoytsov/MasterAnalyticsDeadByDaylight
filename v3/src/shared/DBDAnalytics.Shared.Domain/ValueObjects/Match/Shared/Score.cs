using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared
{
    public readonly record struct Score
    {
        public const int MAX = 50_000;
        public const int MIN = 0;

        public int Value { get; }

        internal Score(int value) => Value = value;

        public static Score From(int value)
        {
            GuardException.Against.That(value > MAX || value < MIN, () => new AggregateException($"Счет может быть в диапазоне от {MIN} до {MAX}"));

            return new Score(value);
        }

        public static implicit operator int(Score score) => score.Value;
    }
}
