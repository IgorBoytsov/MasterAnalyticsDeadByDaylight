using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Responses.Killers;
using DBDAnalytics.Shared.Domain.Constants;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.AddPerk
{
    public sealed class CreateKillerPerkCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateKillerPerkCommand, Result<List<KillerPerkResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<KillerPerkResponse>>> Handle(CreateKillerPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<KillerPerk> addingPerks = [];

                var killerId = request.Perks.FirstOrDefault()?.KillerId;

                if (killerId == null)
                    return Result<List<KillerPerkResponse>>.Failure(new Error(ErrorCode.NotFound, "Киллер не найден, либо нечего добавлять."));

                var killer = await _killerRepository.GetKiller(killerId.Value);

                foreach (var perk in request.Perks)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(perk.Image, FileStoragePaths.KillerPerks(killer.Name), perk.SemanticImageName, cancellationToken);
                    var addPerk = killer.AddPerk(perk.OldId, perk.Name, imageKey, null);
                    addingPerks.Add(addPerk);
                }

                var dto = _mapper.Map<List<KillerPerkResponse>>(addingPerks);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<KillerPerkResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);
                    
                if (ex is DomainException domainEx)
                    return Result<List<KillerPerkResponse>>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<List<KillerPerkResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об перки убийцы {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}