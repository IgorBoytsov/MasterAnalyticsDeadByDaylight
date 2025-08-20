using AutoMapper;
using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.Models;
using DBDAnalytics.Shared.Contracts.Responses;
using DBDAnalytics.Shared.Domain.Exceptions;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Create
{
    public sealed class CreateRoleCommandHandler(
        IApplicationDbContext context,
        IRoleRepository roleRepository,
        IMapper mapper) : IRequestHandler<CreateRoleCommand, Result<RoleResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<RoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rarity = Role.Create(request.OldId, request.Name);

                await _roleRepository.AddAsync(rarity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<RoleResponse>(rarity);

                return Result<RoleResponse>.Success(dto);
            }
            catch (DomainException ex)
            {
                return Result<RoleResponse>.Failure(new Error(ErrorCode.Validation, ex.Message));
            }
            catch (Exception ex)
            {
                return Result<RoleResponse>.Failure(new Error(ErrorCode.Create, $"Произошла непредвиденная ошибка при создание/сохранение записи об роли {ex.Message} {ex.InnerException?.Message}"));
            }
        }
    }
}