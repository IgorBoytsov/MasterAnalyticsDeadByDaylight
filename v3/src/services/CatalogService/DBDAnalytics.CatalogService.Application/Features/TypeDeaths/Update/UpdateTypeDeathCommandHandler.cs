using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.TypeDeath;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Update
{
    public sealed class UpdateTypeDeathCommandHandler(
        IApplicationDbContext context,
        ITypeDeathRepository typeDeathRepository) : IRequestHandler<UpdateTypeDeathCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;

        public async Task<Result> Handle(UpdateTypeDeathCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var typeDeath = await _typeDeathRepository.Get(request.TypeDeathId);

                if (typeDeath is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                typeDeath.UpdateName(TypeDeathName.Create(request.Name));

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Error);
            }
            catch (Exception)
            {
                return Result.Failure(new Error(ErrorCode.Update, "Произошла непредвиденная ошибка на стороне сервера."));
            }
        }
    }
}