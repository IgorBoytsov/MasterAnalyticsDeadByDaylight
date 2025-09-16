using DBDAnalytics.Shared.Domain.Results;

namespace Shared.Api.Application.Guards
{
    public static class GuardRequest
    {
        public static bool TryGetFailureResult<TRequest>(TRequest? request, out Result failureResult)
        {
            if (request is not null)
            {
                failureResult = null!;
                return false;
            }

            var systemMessage = $"Запрос типа '{typeof(TRequest).Name}' не может быть null.";
            var clientMessage = "Ошибка валидации на стороне сервера. Пожалуйста повторите попытку позже.";

            failureResult = Result.Failure(new Error(ErrorCode.Null, clientMessage));
            return true;
        }

        public static bool TryGetFailureResult<TRequest, TResult>(TRequest? request, out Result<TResult> failureResult)
        {
            if (request is not null)
            {
                failureResult = null!;
                return false;
            }
            var systemMessage = $"Запрос типа '{typeof(TRequest).Name}' не может быть null.";
            var clientMessage = "Ошибка валидации на стороне сервера. Пожалуйста повторите попытку позже.";

            failureResult = Result<TResult>.Failure(new Error(ErrorCode.Null, clientMessage));
            return true;
        }
    }
}