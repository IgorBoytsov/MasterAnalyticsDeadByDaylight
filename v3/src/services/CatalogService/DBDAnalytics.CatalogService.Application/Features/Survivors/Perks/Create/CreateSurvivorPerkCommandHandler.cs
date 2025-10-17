using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Image;
using DBDAnalytics.Shared.Contracts.Constants;
using DBDAnalytics.Shared.Contracts.Responses.Survivor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Survivors.Perks.Create
{
    public sealed class CreateSurvivorPerkCommandHandler(
        IApplicationDbContext context,
        ISurvivorRepository survivorRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateSurvivorPerkCommand, Result<List<SurvivorPerkResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ISurvivorRepository _survivorRepository = survivorRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<SurvivorPerkResponse>>> Handle(CreateSurvivorPerkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<SurvivorPerk> addingPerks = [];

                var survivorId = request.Perks.FirstOrDefault()?.SurvivorId;

                if (survivorId == null)
                    return Result<List<SurvivorPerkResponse>>.Failure(new Error(ErrorCode.NotFound, "У выжившего нету идентификатора."));

                #region Проверка на дубликаты записей

                var requestPerksNames = request.Perks
                    .Select(p => p.Name)
                    .ToList();

                var existingPerkNames = await _context.SurvivorPerks
                    .Where(perk => perk.SurvivorId == survivorId && requestPerksNames.Contains(perk.Name))
                    .Select(perk => perk.Name.Value)
                    .ToListAsync(cancellationToken);

                if (existingPerkNames.Any())
                    return Result<List<SurvivorPerkResponse>>.Failure(new Error(ErrorCode.Validation, $"Следующие перки уже существуют для этого выжившего: {string.Join(", ", existingPerkNames)}"));

                #endregion

                var survivor = await _survivorRepository.GetSurvivor(survivorId.Value);

                if (survivor == null)
                    return Result<List<SurvivorPerkResponse>>.Failure(new Error(ErrorCode.NotFound, "Выживший не найден."));

                foreach (var perk in request.Perks)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(perk.Image, FileStoragePaths.SurvivorPerks(survivor.Name), perk.SemanticImageName, cancellationToken);
                    var addPerk = survivor.AddPerk(perk.Name, perk.OldId, imageKey, null);
                    addingPerks.Add(addPerk);
                }

                var dto = _mapper.Map<List<SurvivorPerkResponse>>(addingPerks);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<SurvivorPerkResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<List<SurvivorPerkResponse>>.Failure(domainEx.Error);

                return Result<List<SurvivorPerkResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об перки выжившего {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}