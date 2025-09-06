using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Patch
{
    public readonly record struct PatchId
    {
        public int Value { get; }

        internal PatchId(int value) => Value = value;

        public static PatchId From(int patchId)
        {
            GuardException.Against.That(patchId <= 0, () => new ArgumentException("Id для патча должно быть положительным числом."));

            return new PatchId(patchId);
        }

        public static implicit operator int(PatchId patchId) => patchId.Value;

    }
}