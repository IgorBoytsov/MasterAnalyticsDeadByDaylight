namespace DBDAnalytics.Domain.DomainModels
{
    public class RoleDomain
    {
        private RoleDomain(int idRole, string roleName, string? roleDescription)
        {
            IdRole = idRole;
            RoleName = roleName;
            RoleDescription = roleDescription;
        }
        public int IdRole { get; private set; }

        public string RoleName { get; private set; } = null!;

        public string? RoleDescription { get; private set; }

        public static (RoleDomain? RoleDomain, string? Message) Create(int idRole, string roleName, string? roleDescription)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(roleName))
                return (null, "Вы не дали название роли");

            var role = new RoleDomain(idRole, roleName, roleDescription);

            return (role, message);
        }
    }
}