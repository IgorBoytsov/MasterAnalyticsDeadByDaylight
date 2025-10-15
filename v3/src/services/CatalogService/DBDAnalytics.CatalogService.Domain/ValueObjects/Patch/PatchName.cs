using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Domain.ValueObjects.Patch
{
    public sealed record PatchName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; }

        internal PatchName(string value) => Value = value;

        /// <exception cref="NameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static PatchName Create(string patchName)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(patchName), () => new NameException(new Error(ErrorCode.Validation, "Название патча не может быть пустым.")));
            GuardException.Against.That(patchName.Length > MAX_LENGTH, () => new LengthException(new Error(ErrorCode.Validation, $"Допустима длина для патча {MAX_LENGTH} символов")));

            return new PatchName(patchName);
        }

        public override string ToString() => Value;

        public static implicit operator string(PatchName patchName) => patchName.Value;
    }
}