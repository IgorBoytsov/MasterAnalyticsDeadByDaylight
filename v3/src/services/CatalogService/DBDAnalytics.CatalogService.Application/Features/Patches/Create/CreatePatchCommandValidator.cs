using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Exceptions;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Create
{
    public sealed class CreatePatchCommandValidator : AbstractValidator<CreatePatchCommand>
    {
        private readonly IPatchRepository _patchRepository;

        public CreatePatchCommandValidator(IPatchRepository patchRepository)
        {
            _patchRepository = patchRepository;

            Include(new NameValidator<CreatePatchCommand>(PatchName.MAX_LENGTH));

            When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
            {
                RuleFor(x => x.Name)
                    .MustAsync(async (name, clt) => !await _patchRepository.Exist(name)).WithMessage("Данное название патча уже занято.");
            });

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Дата патча обязательна для заполнения.")
                .Custom((date, context) =>
                {
                    try
                    {
                        PatchDate.Create(date);
                    }
                    catch (InvalidPatchDateException ex)
                    {
                        context.AddFailure(ex.Message);
                    }
                })
                .MustAsync(async (date, cancellationToken) => !await _patchRepository.ExistDate(date))
                .WithMessage("В данную дату патч уже выходил.");
        }
    }
}