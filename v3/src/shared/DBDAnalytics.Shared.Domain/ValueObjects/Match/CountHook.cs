using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match
{
    public readonly record struct CountHook
    {
        public const int MAX_COUNT = 12;
        public const int MIN_COUNT = 0;

        public readonly int Value;

        internal CountHook(int value) => Value = value;

        public static CountHook From(int value)
        {
            GuardException.Against.That(value > MAX_COUNT || value < MIN_COUNT, () => new ArgumentException($"Кол-во хуков может быть в диапазоне между {MIN_COUNT} и {MAX_COUNT}"));

            return new CountHook(value);
        }

        public static implicit operator int(CountHook countHook) => countHook.Value;
    }
}