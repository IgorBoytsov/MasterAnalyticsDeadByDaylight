using DBDAnalytics.CatalogService.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Patch
{
    public readonly record struct PatchDate
    {
        public static readonly DateTime MinValue = new(2016, 6, 14, 0, 0, 0, DateTimeKind.Utc);

        public DateTime Value { get; }

        internal PatchDate(DateTime value) => Value = value;

        /// <exception cref="InvalidPatchDateException"></exception>
        public static PatchDate Create(DateTime dateTime)
        {
            var utcDate = dateTime.ToUniversalTime();

            GuardException.Against.That(utcDate > DateTime.UtcNow, () => new InvalidPatchDateException(new Error( ErrorCode.Validation, "Дата патча не может быть в будущем.")));
            GuardException.Against.That(utcDate > DateTime.UtcNow, () => new InvalidPatchDateException(new Error(ErrorCode.Validation, $"Дата патча не может быть раньше {MinValue:yyyy-MM-dd}.")));

            return new PatchDate(utcDate);
        }

        public static PatchDate Now() => new(DateTime.UtcNow);

        public bool IsOlderThan(TimeSpan period) => (DateTime.UtcNow - Value) > period;

        public override string ToString() => Value.ToString("o");

        public static explicit operator DateTime(PatchDate patchDate) => patchDate.Value;
    }
}