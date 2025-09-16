using FluentValidation.Results;

namespace Shared.Api.Application.Services
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T model);
    }
}