using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses.CharacterInfo;
using MediatR;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.TypeDeaths.Create
{
    public sealed class CreateTypeDeathCommandHandler(
        IApplicationDbContext context,
        ITypeDeathRepository typeDeathRepository,
        IMapper mapper) : IRequestHandler<CreateTypeDeathCommand, Result<TypeDeathResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly ITypeDeathRepository _typeDeathRepository = typeDeathRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<TypeDeathResponse>> Handle(CreateTypeDeathCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var typeDeath = TypeDeath.Create(request.OldId, request.Name);

                await _typeDeathRepository.AddAsync(typeDeath, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<TypeDeathResponse>(typeDeath);

                return Result<TypeDeathResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<TypeDeathResponse>.Failure(new Error(ErrorCode.Validation, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<TypeDeathResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об типе смерти {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}