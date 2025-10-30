using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match
{
    public readonly record struct CountKill
    {
        public const int MAX_COUNT = 4;
        public const int MIN_COUNT = 0;

        public readonly int Value;

        internal CountKill(int value) => Value = value;

        public static CountKill From(int value)
        {
            GuardException.Against.That(value > MAX_COUNT || value < MIN_COUNT, () => new ArgumentException($"Кол-во убийств может быть в диапазоне между {MIN_COUNT} и {MAX_COUNT}"));

            return new CountKill(value);
        }

        public static implicit operator int(CountKill countKill) => countKill.Value;
    }
}