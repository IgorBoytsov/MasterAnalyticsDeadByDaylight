using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class RoleDTO : BaseDTO<RoleDTO>
    {
        public int IdRole { get; set; }

        public string RoleName { get; set; } = null!;

        public string? RoleDescription { get; set; }
    }
}
