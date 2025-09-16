using DBDAnalytics.Shared.Domain.Results;
using FluentValidation.Results;

namespace Shared.Api.Application.Guards
{
    public static class GuardValidation
    {
        public static bool TryGetFailureResult<TResult>(ValidationResult validationResult, out Result<TResult> failureResult)
        {
            if (validationResult.IsValid)
            {
                failureResult = null!;
                return false;
            }

            var validationErrors = validationResult.Errors.Select(e => new Error(ErrorCode.Validation, $"Ошибка валидации: {e.ErrorMessage}"));
            failureResult = Result<TResult>.Failure(validationErrors);
            return true;
        }

        public static bool TryGetFailureResult(ValidationResult validationResult, out Result failureResult)
        {
            if (validationResult.IsValid)
            {
                failureResult = null!;
                return false;
            }

            var validationErrors = validationResult.Errors.Select(e => new Error(ErrorCode.Validation, $"Ошибка валидации: {e.ErrorMessage}")).ToList();
            failureResult = Result.Failure(validationErrors);
            return true;
        }
    }
}