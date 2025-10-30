using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match
{
    public readonly record struct RecentGenerator
    {
        public const int MAX_COUNT = 5;
        public const int MIN_COUNT = 0;

        public readonly int Value;

        internal RecentGenerator(int value) => Value = value;

        public static RecentGenerator From(int value)
        {
            GuardException.Against.That(value > MAX_COUNT || value < MIN_COUNT, () => new ArgumentException($"Кол-во оставшихся генераторов может быть в диапазоне между {MIN_COUNT} и {MAX_COUNT}"));

            return new RecentGenerator(value);
        }

        public static implicit operator int(RecentGenerator recentGenerator) => recentGenerator.Value;
    }
}
