using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Patch;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Update
{
    public sealed class UpdatePatchCommandHandler(
        IApplicationDbContext context,
        IPatchRepository patchRepository) : IRequestHandler<UpdatePatchCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<Result> Handle(UpdatePatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patch = await _patchRepository.Get(request.Id);

                if (patch is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                patch.UpdateName(PatchName.Create(request.Name));
                patch.UpdateDate(PatchDate.Create(request.Date));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}