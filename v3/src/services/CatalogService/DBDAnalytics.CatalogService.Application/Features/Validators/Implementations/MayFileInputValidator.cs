using DBDAnalytics.CatalogService.Application.Features.Validators.Abstractions;
using FluentValidation;

namespace DBDAnalytics.CatalogService.Application.Features.Validators.Implementations
{
    public sealed class MayFileInputValidator : AbstractValidator<IMayHasFileInput>
    {
        private const int MaxFileSizeInBytes = 5 * 1024 * 1024;
        private readonly IList<string> _allowedExtensions = [".jpg", ".jpeg", ".png"];
        private readonly IList<string> _allowedContentTypes = ["image/jpeg", "image/png"];

        public MayFileInputValidator()
        {
            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                    .NotEmpty().WithMessage("Имя файла не может быть пустым.")
                    .MaximumLength(255).WithMessage("Имя файла слишком длинное.")
                    .Must(BeAValidFileName).WithMessage("Имя файла содержит недопустимые символы.")
                    .Must(HaveAllowedExtension).WithMessage($"Недопустимое расширение файла. Разрешены: {string.Join(", ", _allowedExtensions)}");

                RuleFor(x => x.Image!.ContentType)
                    .NotEmpty().WithMessage("Тип контента не может быть пустым.")
                    .Must(ct => _allowedContentTypes.Contains(ct.ToLowerInvariant()))
                    .WithMessage($"Недопустимый тип контента. Разрешены: {string.Join(", ", _allowedContentTypes)}");

                RuleFor(x => x.Image!.Content)
                    .NotEmpty().WithMessage("Содержимое файла не может быть пустым.");

                RuleFor(x => x.Image!.Content.Length)
                    .LessThanOrEqualTo(MaxFileSizeInBytes)
                    .WithMessage($"Размер файла не должен превышать {MaxFileSizeInBytes / 1024 / 1024} МБ.")
                    .When(x => x.Image!.Content != null && x.Image!.Content.CanSeek, ApplyConditionTo.CurrentValidator); 

            });
        }

        private bool BeAValidFileName(string fileName) => !string.IsNullOrWhiteSpace(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;

        private bool HaveAllowedExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return false;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return !string.IsNullOrEmpty(extension) && _allowedExtensions.Contains(extension);
        }
    }
}