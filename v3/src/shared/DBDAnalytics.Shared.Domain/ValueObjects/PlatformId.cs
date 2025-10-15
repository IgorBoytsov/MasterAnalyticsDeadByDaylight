using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct PlatformId
    {
        public int Value { get; }

        internal PlatformId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static PlatformId From(int platformId)
        {
            GuardException.Against.That(platformId <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id игровой платформы должен быть положительным числом.")));

            return new PlatformId(platformId);
        }

        public static implicit operator int(PlatformId platformId) => platformId.Value;
    }
}