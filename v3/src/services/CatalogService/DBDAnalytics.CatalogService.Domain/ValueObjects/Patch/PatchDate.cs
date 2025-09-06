using DBDAnalytics.CatalogService.Domain.Exceptions;

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

            if (utcDate > DateTime.UtcNow)
                throw new InvalidPatchDateException("Дата патча не может быть в будущем.");

            if (utcDate < MinValue)
                throw new InvalidPatchDateException($"Дата патча не может быть раньше {MinValue:yyyy-MM-dd}.");

            return new PatchDate(utcDate);
        }

        public static PatchDate Now() => new(DateTime.UtcNow);

        public bool IsOlderThan(TimeSpan period) => (DateTime.UtcNow - Value) > period;

        public override string ToString() => Value.ToString("o");

        public static explicit operator DateTime(PatchDate patchDate) => patchDate.Value;
    }
}