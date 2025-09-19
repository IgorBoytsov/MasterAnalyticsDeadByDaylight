using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.KillerPerkCategories.Create
{
    public sealed class CreateKillerPerkCategoryCommandHandler(
        IApplicationDbContext context,
        IKillerPerkCategoryRepository killerPerkCategoryRepository,
        IMapper mapper) : IRequestHandler<CreateKillerPerkCategoryCommand, Result<KillerPerkCategoryResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerPerkCategoryRepository _killerPerkCategoryRepository = killerPerkCategoryRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<KillerPerkCategoryResponse>> Handle(CreateKillerPerkCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _killerPerkCategoryRepository.Exist(request.Name))
                    return Result<KillerPerkCategoryResponse>.Failure(new Error(ErrorCode.Exist, $"{request.Name} категория уже существует."));

                var category = KillerPerkCategory.Create(request.OldId, request.Name);

                await _killerPerkCategoryRepository.AddAsync(category, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<KillerPerkCategoryResponse>(category);

                return Result<KillerPerkCategoryResponse>.Success(dto);
            }
            catch(DomainException ex)
            {
                return Result<KillerPerkCategoryResponse>.Failure(new Error(ErrorCode.Domain, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<KillerPerkCategoryResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об категории у перка киллера {ex.Message}"));
            }
        }
    }
}