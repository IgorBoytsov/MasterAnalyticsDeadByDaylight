using DBDAnalytics.Application.DTOs;
using DBDAnalytics.Domain.DomainModels;

namespace DBDAnalytics.Application.Mappers
{
    internal static class RolesMapper
    {
        public static RoleDTO ToDTO(this RoleDomain role)
        {
            return new RoleDTO
            {
                IdRole = role.IdRole,
                RoleDescription = role.RoleDescription,
                RoleName = role.RoleName,
            };
        }

        public static List<RoleDTO> ToDTO(this IEnumerable<RoleDomain> roles)
        {
            var list = new List<RoleDTO>();

            foreach (var role in roles)
            {
                list.Add(role.ToDTO());
            }

            return list;
        }
    }
}