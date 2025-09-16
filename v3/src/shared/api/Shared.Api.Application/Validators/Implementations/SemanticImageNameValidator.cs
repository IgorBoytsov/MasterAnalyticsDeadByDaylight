using FluentValidation;
using Shared.Api.Application.Validators.Abstractions;

namespace Shared.Api.Application.Validators.Implementations
{
    public sealed class SemanticImageNameValidator<T> : AbstractValidator<T> where T : IHasSemanticImageName
    {
        public static readonly char[] InvalidChars =
        {
            ';', ':',
            '<', '>',
            '/', '\\', '|',
        };

        public SemanticImageNameValidator()
         => RuleFor(x => x.SemanticImageName)
                .NotEmpty().WithMessage("Семантическое название для картинки не может быть пустым.")
                .Must(name =>
                {
                    if (string.IsNullOrEmpty(name))
                        return true;

                    return name.IndexOfAny(InvalidChars) == -1;
                });
    }
}