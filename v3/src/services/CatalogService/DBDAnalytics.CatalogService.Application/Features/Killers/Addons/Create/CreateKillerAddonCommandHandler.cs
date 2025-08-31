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
using Microsoft.EntityFrameworkCore;

namespace DBDAnalytics.CatalogService.Application.Features.Killers.Addons.Create
{
    public sealed class CreateKillerAddonCommandHandler(
        IApplicationDbContext context,
        IKillerRepository killerRepository,
        IMapper mapper,
        IFileUploadManager fileUploadManager) : IRequestHandler<CreateKillerAddonCommand, Result<List<KillerAddonResponse>>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IKillerRepository _killerRepository = killerRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IFileUploadManager _fileUploadManager = fileUploadManager;

        public async Task<Result<List<KillerAddonResponse>>> Handle(CreateKillerAddonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<KillerAddon> addingAddons = [];

                var killerId = request.Addons.FirstOrDefault()?.KillerId;

                if (killerId == null)
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.NotFound, "У убийцы нету идентификатора."));

                #region Проверка на дубликаты записей

                var requestAddonsNames = request.Addons
                    .Select(a => a.Name)
                    .ToList();

                var existingAddonNames = await _context.KillerAddons
                    .Where(addon => addon.KillerId == killerId && requestAddonsNames.Contains(addon.Name))
                    .Select(addon => addon.Name)
                    .ToListAsync(cancellationToken);

                if (existingAddonNames.Any())
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.Validation, $"Следующие улучшения уже существуют для этого убийцы: {string.Join(", ", existingAddonNames)}"));

                #endregion

                var killer = await _killerRepository.GetKiller(killerId.Value);

                if (killer == null)
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.NotFound, "Киллер не найден."));

                foreach (var addon in request.Addons)
                {
                    ImageKey? imageKey = await _fileUploadManager.UploadImageAsync(addon.Image, FileStoragePaths.KillerAddons(killer.Name), addon.SemanticImageName, cancellationToken);
                    var addAddon = killer.AddAddon(addon.OldId, addon.Name, imageKey);
                    addingAddons.Add(addAddon);
                }

                var dto = _mapper.Map<List<KillerAddonResponse>>(addingAddons);

                await _context.SaveChangesAsync(cancellationToken);

                return Result<List<KillerAddonResponse>>.Success(dto);
            }
            catch (Exception ex)
            {
                await _fileUploadManager.RollbackUploadsAsync(cancellationToken);

                if (ex is DomainException domainEx)
                    return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.Validation, domainEx.Message));

                return Result<List<KillerAddonResponse>>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об улучшение убийцы {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}