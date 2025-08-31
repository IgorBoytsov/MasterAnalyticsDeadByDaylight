using DBDAnalytics.CatalogService.Application.Features.Validators.Implementations;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Map;
using FluentValidation;
using Shared.Api.Application.Validators.Implementations;

namespace DBDAnalytics.CatalogService.Application.Features.Measurements.Maps.Update
{
    public sealed class UpdateMapCommandValidator : AbstractValidator<UpdateMapCommand>
    {
        public UpdateMapCommandValidator()
        {
            Include(new NameValidator<UpdateMapCommand>(MapName.MAX_LENGTH));
            Include(new SemanticImageNameValidator<UpdateMapCommand>());
            Include(new MayFileInputValidator());
        }
    }
}