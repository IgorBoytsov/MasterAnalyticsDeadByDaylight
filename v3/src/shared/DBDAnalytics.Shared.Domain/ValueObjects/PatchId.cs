using DBDAnalytics.Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions.Guard;
using Shared.Kernel.Results;

namespace DBDAnalytics.Shared.Domain.ValueObjects
{
    public readonly record struct PatchId
    {
        public int Value { get; }

        internal PatchId(int value) => Value = value;

        /// <exception cref="IdentifierOutOfRangeException"></exception>
        public static PatchId From(int patchId)
        {
            GuardException.Against.That(patchId <= 0, () => new IdentifierOutOfRangeException(new Error(ErrorCode.InvalidIdentifier, "Id патча должен быть положительным числом.")));

            return new PatchId(patchId);
        }

        public static implicit operator int(PatchId patchId) => patchId.Value;

    }
}