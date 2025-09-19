using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Patches.Delete
{
    public sealed class DeletePatchCommandHandler(
        IApplicationDbContext context,
        IPatchRepository patchRepository) : IRequestHandler<DeletePatchCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPatchRepository _patchRepository = patchRepository;

        public async Task<Result> Handle(DeletePatchCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var patch = await _patchRepository.Get(request.Id);

                if (patch is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _patchRepository.Remove(patch);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Delete, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}