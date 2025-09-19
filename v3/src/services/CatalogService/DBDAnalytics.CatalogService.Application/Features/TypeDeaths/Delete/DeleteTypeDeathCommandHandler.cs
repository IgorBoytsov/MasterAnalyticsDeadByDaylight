using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Delete
{
    public sealed class DeleteTypeDeathCommandHandler(
        IApplicationDbContext context,
        ITypeDeathRepository typeDeathRepository) : IRequestHandler<DeleteTypeDeathCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<Result> Handle(DeleteTypeDeathCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var typeDeath = await _typeDeathRepository.Get(request.Id);

                if (typeDeath is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _typeDeathRepository.Remove(typeDeath);

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