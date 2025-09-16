using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class NameValidator<T> : AbstractValidator<T> where T : IHasName
    {
        public static readonly char[] InvalidChars =
        {
            ';', ':',
            '<', '>',
            '/', '\\', '|',
        };

        public NameValidator(int maxLength) 
            => RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название не может быть пустым.")
                .MaximumLength(maxLength).WithMessage($"Максимально допустима длинна имени {maxLength} символов.")
                .Must(name =>
                {
                    if (string.IsNullOrEmpty(name))
                        return true;

                    return name.IndexOfAny(InvalidChars) == -1;
                });
    }
}