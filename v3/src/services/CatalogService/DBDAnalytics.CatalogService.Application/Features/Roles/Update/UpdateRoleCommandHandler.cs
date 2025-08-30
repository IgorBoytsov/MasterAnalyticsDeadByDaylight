using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using DBDAnalytics.CatalogService.Domain.ValueObjects.Role;
using DBDAnalytics.Shared.Domain.Results;
using MediatR;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Update
{
    public sealed class UpdateRoleCommandHandler(
        IApplicationDbContext context,
        IRoleRepository roleRepository) : IRequestHandler<UpdateRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleRepository.Get(request.RoleId);

                if (role is null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной записи не найдено."));

                role.UpdateName(RoleName.Create(request.Name));

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