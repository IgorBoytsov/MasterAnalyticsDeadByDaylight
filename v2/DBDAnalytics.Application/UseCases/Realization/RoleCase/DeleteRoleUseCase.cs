using DBDAnalytics.Application.UseCases.Abstraction.RoleCase;
using DBDAnalytics.Domain.Interfaces.Repositories;

namespace DBDAnalytics.Application.UseCases.Realization.RoleCase
{
    public class DeleteRoleUseCase(IRoleRepository roleRepository) : IDeleteRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<(bool IsDeleted, string Message)> DeleteAsync(int idRole)
        {
            string message = string.Empty;

            var existBeforeDelete = await _roleRepository.ExistAsync(idRole);

            if (!existBeforeDelete)
                return (false, "Такой записи нету.");

            var id = await _roleRepository.DeleteAsync(idRole);

            var existAfterDelete = await _roleRepository.ExistAsync(id);

            if (!existAfterDelete)
                return (true, "Элемент удален");
            return (true, "Элемент удален");
        }
    }
}