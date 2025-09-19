using Shared.Kernel.Exceptions.Guard;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct PlatformId
    {
        public int Value { get; }

        internal PlatformId(int value) => Value = value;

        public static PlatformId From(int platformId)
        {
            GuardException.Against.That(platformId <= 0, () => new ArgumentException("Id для платформы должно быть положительным числом."));

            return new PlatformId(platformId);
        }

        public static implicit operator int(PlatformId platformId) => platformId.Value;
    }
}