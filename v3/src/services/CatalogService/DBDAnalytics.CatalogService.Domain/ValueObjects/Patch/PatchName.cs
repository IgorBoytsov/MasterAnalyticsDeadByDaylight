using DBDAnalytics.Shared.Domain.Exceptions.Guard;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Patch
{
    public sealed record PatchName
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; }

        internal PatchName(string value) => Value = value;

        public static PatchName Create(string patchName)
        {
            GuardException.Against.NullOrWhiteSpace(patchName, nameof(patchName));
            GuardException.Against.That(patchName.Length > MAX_LENGTH, () => new ArgumentOutOfRangeException($"Допустима длина для патча {MAX_LENGTH} символов"));

            return new PatchName(patchName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PatchName patchName) => patchName.Value;
    }
}