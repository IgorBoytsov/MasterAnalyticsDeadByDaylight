using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Offerings.Delete
{
    public sealed class DeleteOfferingCommandValidator : AbstractValidator<DeleteOfferingCommand>
    {
        public DeleteOfferingCommandValidator() => Include(new GuidValidator<DeleteOfferingCommand>());
    }
}