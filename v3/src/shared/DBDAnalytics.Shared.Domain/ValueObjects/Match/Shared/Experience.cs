using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects.Match.Shared
{
    public readonly record struct Experience
    {
        public const int MAX = 600;
        public const int MIN = 0;

        public int Value { get; }

        internal Experience(int value) => Value = value;

        public static Experience From(int value)
        {
            GuardException.Against.That(value > MAX || value < MIN, () => new AggregateException($"Опыт может быть в диапазоне от {MIN} до {MAX}"));

            return new Experience(value);
        }

        public static implicit operator int(Experience experience) => experience.Value;
    }
}