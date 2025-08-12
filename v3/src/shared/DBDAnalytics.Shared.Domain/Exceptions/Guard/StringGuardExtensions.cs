using System.Diagnostics.CodeAnalysis;

namespace DBDAnalytics.Shared.Domain.Exceptions.Guard
{
    public static class StringGuardExtensions
    {
        /// <summary>
        /// Проверяет, является ли строка null, пустой или состоит только из пробелов.
        /// </summary>
        /// <returns>Входная строка, если проверка пройдена.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string NullOrWhiteSpace(this IGuardClause _, [NotNull] string input, string parameterName, string? message = null)
        {
            GuardException.Against.Null(input, parameterName, message);

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(message ?? $"Параметр {parameterName} не может быть пустым или состоять из пустых символов.", parameterName);
            }

            return input;
        }
    }
}