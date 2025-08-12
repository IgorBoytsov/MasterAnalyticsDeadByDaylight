namespace DBDAnalytics.Shared.Domain.Exceptions.Guard
{
    public static class NullGuardExtensions
    {
        /// <summary>
        /// Проверяет, является ли входной объект null, и выбрасывает ArgumentNullException, если это так.
        /// </summary>
        /// <typeparam name="T">Тип объекта.</typeparam>
        /// <param name="guardClause">Экземпляр Guard.</param>
        /// <param name="input">Проверяемый объект.</param>
        /// <param name="parameterName">Имя параметра.</param>
        /// <param name="message">Опциональное сообщение об ошибке.</param>
        /// <returns>Входной объект, если он не null.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Null<T>(this IGuardClause _, T? input, string parameterName, string? message = null)
        {
            if (input is null)
                throw new ArgumentNullException(parameterName, message ?? $"Параметре {parameterName} не может быть null.");

            return input;
        }
    }
}