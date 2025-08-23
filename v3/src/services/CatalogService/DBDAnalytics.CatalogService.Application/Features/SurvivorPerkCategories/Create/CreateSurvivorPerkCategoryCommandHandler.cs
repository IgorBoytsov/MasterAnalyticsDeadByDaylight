using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.SurvivorPerkCategories.Create
{
    public sealed class CreateSurvivorPerkCategoryCommandHandler(
        IApplicationDbContext context,
        ISurvivorPerkCategoryRepository survivorPerkCategoryRepositor,
        IMapper mapper) : IRequestHandler<CreateSurvivorPerkCategoryCommand, Result<SurvivorPerkCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorPerkCategoryRepository _survivorPerkCategoryRepositor = survivorPerkCategoryRepositor;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<SurvivorPerkCategoryResponse>> Handle(CreateSurvivorPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _survivorPerkCategoryRepositor.Exist(request.Name))
                    return Result<SurvivorPerkCategoryResponse>.Failure(new Error(ErrorCode.Exist, $"{request.Name} категория уже существует."));

                var category = SurvivorPerkCategory.Create(request.OldId, request.Name);

                await _survivorPerkCategoryRepositor.AddAsync(category, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<SurvivorPerkCategoryResponse>(category);

                return Result<SurvivorPerkCategoryResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<SurvivorPerkCategoryResponse>.Failure(new Error(ErrorCode.Domain, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<SurvivorPerkCategoryResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об категории у перка выжившего {ex.Message}"));
            }
        }
    }
}