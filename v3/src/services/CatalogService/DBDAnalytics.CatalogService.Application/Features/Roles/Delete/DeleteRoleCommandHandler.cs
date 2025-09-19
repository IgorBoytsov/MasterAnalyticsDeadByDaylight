using DBDAnalytics.CatalogService.Application.Common.Abstractions;
using DBDAnalytics.CatalogService.Application.Common.Repository;
using MediatR;
using Shared.Kernel.Results;

namespace DBDAnalytics.CatalogService.Application.Features.Roles.Delete
{
    public sealed class DeleteRoleCommandHandler(
        IApplicationDbContext context,
        IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleRepository.Get(request.Id);

                if (role is null)
                    return Result.Failure(new Error(ErrorCode.Delete, "Такой записи не существует."));

                _roleRepository.Remove(role);

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