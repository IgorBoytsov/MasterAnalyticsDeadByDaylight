using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Rarities.Create
{
    public sealed class CreateRarityCommandHandler(
		IApplicationDbContext context, 
		IRarityRepository rarityRepository, 
		IMapper mapper) : IRequestHandler<CreateRarityCommand, Result<RarityResponse>>
    {
		private readonly IApplicationDbContext _context = context;
		private readonly IRarityRepository _rarityRepository = rarityRepository;
		private readonly IMapper _mapper = mapper;

        public async Task<Result<RarityResponse>> Handle(CreateRarityCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var rarity = Rarity.Create(request.OldId, request.Name);

				await _rarityRepository.AddAsync(rarity, cancellationToken);
				await _context.SaveChangesAsync(cancellationToken);

				var dto = _mapper.Map<RarityResponse>(rarity);

				return Result<RarityResponse>.Success(dto);
			}
			catch (DomainException ex)
			{
                return Result<RarityResponse>.Failure(ex.Error);
            }
			catch (Exception ex)
            {
                return Result<RarityResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об качестве {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}