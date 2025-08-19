using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Measurement;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Create
{
    public sealed class CreateMeasurementCommandValidator : AbstractValidator<CreateMeasurementCommand>
    {
        private readonly IMeasurementRepository _measurementRepository;

        public CreateMeasurementCommandValidator(IMeasurementRepository measurementRepository)
        {
            _measurementRepository = measurementRepository;

            Include(new NameValidator<CreateMeasurementCommand>(MeasurementName.MAX_LENGTH));

            RuleForEach(x => x.Maps)
                .SetValidator(new CreateMapCommandDataValidator());

            RuleFor(x => x.Maps)
                .Must(maps => maps.GroupBy(m => m.Name.ToLower()).All(g => g.Count() == 1)).WithMessage("Найдены дубликаты карт по имени в одном запросе.");

            When(m => !string.IsNullOrWhiteSpace(m.Name), () =>
            {
                RuleFor(m => m.Name)
                    .MustAsync(async (name, clt) => !await _measurementRepository.Exist(name)).WithMessage($"Измерение с таким именем уже существует");
            });
        }
    }

    public sealed class CreateMapCommandDataValidator : AbstractValidator<CreateMapCommandData>
    {
        public CreateMapCommandDataValidator()
        {
            Include(new NameValidator<CreateMapCommandData>(MapName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<CreateMapCommandData>());

            When(m => m.Image != null, () =>
            {
                RuleFor(x => x.Image!.FileName)
                    .NotEmpty().WithMessage("Имя файла улучшения не может быть пустым");

                RuleFor(x => x.Image!.Content)
                    .NotEmpty().WithMessage("Содержимое файла улучшения не может быть пустым.");
            });
        }
    }
}