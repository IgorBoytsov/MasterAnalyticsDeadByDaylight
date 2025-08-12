using DBDAnalytics.Shared.Domain.Exceptions.Guard;
using System.Text;

namespace DBDAnalytics.Shared.Domain.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        private Result(TValue? value, bool isSuccess, List<Error> errors) : base(isSuccess, errors)
        {
            GuardException.Against.That(
                isSuccess && value is null && default(TValue) is not null, 
                () => new InvalidOperationException("Нельзя создать успешный результат для типа значения (value type), не допускающего null, с значением null."));

            _value = value;
        }

        /// <summary>
        /// Возвращает значение в случае успеха.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Нельзя получить значение из неуспешного результата.");

        /*--Фабричные методы------------------------------------------------------------------------------*/

        public static Result<TValue> Success(TValue value) => new(value, true, []);

        public new static Result<TValue> Failure(Error error) => new(default, false, [error]);

        public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(default, false, errors.ToList());

        /// <summary>
        /// Выполняет одну из двух функций в зависимости от состояния результата и возвращает их результат.
        /// </summary>
        public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Errors);

        /// <summary>
        /// Выполняет одно из двух действий в зависимости от состояния результата.
        /// </summary>
        public void Switch(Action<TValue> onSuccess, Action<IReadOnlyList<Error>> onFailure)
        {
            if (IsSuccess)
                onSuccess(Value);
            else
                onFailure(Errors);
        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, List<Error> errors)
        {
            GuardException.Against.That(isSuccess && errors.Any(), () => new InvalidOperationException("Нельзя создать успешный результат с ошибками."));
            GuardException.Against.That(!isSuccess && !errors.Any(), () => new InvalidOperationException("Нельзя создать провальный результат без ошибок."));

            IsSuccess = isSuccess;
            Errors = errors;
        }

        /*--Фабричные методы------------------------------------------------------------------------------*/

        public static Result Success() => new(true, []);
        public static Result Failure(Error error) => new(false, [error]);
        public static Result Failure(IEnumerable<Error> errors) => new(false, errors.ToList());

        /// <summary>
        /// Выполняет одну из двух функций в зависимости от состояния результата и возвращает их результат.
        /// </summary>
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onFailure) => IsSuccess ? onSuccess() : onFailure(Errors);

        /// <summary>
        /// Выполняет одно из двух действий в зависимости от состояния результата.
        /// </summary>
        public void Switch(Action onSuccess, Action<IReadOnlyList<Error>> onFailure)
        {
            if (IsSuccess)
                onSuccess();
            else
                onFailure(Errors);
        }

        /*--Отображение ошибки в виде строки--------------------------------------------------------------*/

        public string StringMessage => BuildMessage(error => $"Код: {(int)error.Code} - {error.Code}. Причина: {error.Message}");

        private string BuildMessage(Func<Error, string> messageSelector)
        {
            if (Errors.Count == 0) return "Ошибок нет.";

            var sb = new StringBuilder();
            sb.AppendLine("Ошибки:");

            for (int i = 0; i < Errors.Count; i++)
            {
                string message = messageSelector(Errors[i]);
                sb.Append($"{i + 1}) ").AppendLine(message);
            }

            return sb.ToString();
        }

        /*--Переопределенные методы-----------------------------------------------------------------------*/

        /// <summary>
        /// Выводит полную информацию об ошибках.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => BuildMessage(error => $"Статус: [{IsSuccess}] Код: [{error.Code} - {(int)error.Code}] Информация: [{error.Message}]");
    }
}