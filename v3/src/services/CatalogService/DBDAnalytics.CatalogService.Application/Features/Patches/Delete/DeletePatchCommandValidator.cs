using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Delete
{
    public sealed class DeletePatchCommandValidator : AbstractValidator<DeletePatchCommand>
    {
        public DeletePatchCommandValidator() => Include(new IntValidator<DeletePatchCommand>());
    }
}