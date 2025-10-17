using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Offering;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.OfferingCategories.Create
{
    public sealed class CreateOfferingCategoryCommandHandler(
        IApplicationDbContext context,
        IOfferingCategoryRepository offeringCategoryRepository,
        IMapper mapper) : IRequestHandler<CreateOfferingCategoryCommand, Result<OfferingCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IOfferingCategoryRepository _offeringCategoryRepository = offeringCategoryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<OfferingCategoryResponse>> Handle(CreateOfferingCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = OfferingCategory.Create(request.OldId, request.Name);

                await _offeringCategoryRepository.AddAsync(category, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<OfferingCategoryResponse>(category);

                return Result<OfferingCategoryResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<OfferingCategoryResponse>.Failure(ex.Error);
            }
            catch (Exception ex)
            {
                return Result<OfferingCategoryResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об категории подношения {ex.Message}"));
            }
        }
    }
}