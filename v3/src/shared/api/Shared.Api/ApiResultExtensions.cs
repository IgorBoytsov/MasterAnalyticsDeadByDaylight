using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Kernel.Results;

namespace Shared.Api
{
    public static class ApiResultExtensions
    {
        public static IActionResult ToActionResult(this Result result, Func<IActionResult> onSuccess)
            => result.Match(onSuccess, HandleFailure);

        public static IActionResult ToActionResult<TResultValue>(this Result<TResultValue> result, Func<TResultValue, IActionResult> onSuccess)
            => result.Match(onSuccess, HandleFailure);

        private static IActionResult HandleFailure(IReadOnlyList<Error> errors)
        {
            if (errors is null || errors.Count == 0)
            {
                var fallbackResponse = new
                {
                    Title = "Серверная ошибка",
                    ErrorCode = (int)ErrorCode.Server,
                    Errors = new List<string> { "Произошла неопределенная ошибка." },
                    Date = DateTime.UtcNow
                };
                return new ObjectResult(fallbackResponse) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            var primaryError = errors[0]; 

            var errorResponse = new
            {
                Title = GetErrorTitle(primaryError.Code),
                ErrorCode = (int)primaryError.Code,
                Errors = errors.Select(e => e.Message).ToList(), 
                Date = DateTime.UtcNow
            };

            int statusCode = primaryError.Code switch
            {
                ErrorCode.Validation => StatusCodes.Status400BadRequest,
                ErrorCode.NotFound => StatusCodes.Status404NotFound, 
                ErrorCode.Conflict => StatusCodes.Status409Conflict,

                _ => StatusCodes.Status500InternalServerError
            };

            return new ObjectResult(errorResponse) { StatusCode = statusCode };
        }

        private static string GetErrorTitle(ErrorCode code) => code switch
        {
            ErrorCode.Validation => "Ошибки валидации данных",
            ErrorCode.NotFound => "Ресурс не найден",
            ErrorCode.Conflict => "Конфликт данных",

            _ => "Серверная ошибка"
        };
    }
}