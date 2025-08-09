namespace DBDAnalytics.Domain.DomainModels
{
    public class KillerBuildDomain
    {
        private KillerBuildDomain(int idBuild, int idKiller, string? name, int idPerk1, int idPerk2, int idPerk3, int idPerk4, int idAddon1, int idAddon2)
        {
            IdBuild = idBuild;
            IdKiller = idKiller;
            Name = name;
            IdPerk1 = idPerk1;
            IdPerk2 = idPerk2;
            IdPerk3 = idPerk3;
            IdPerk4 = idPerk4;
            IdAddon1 = idAddon1;
            IdAddon2 = idAddon2;
        }
        public int IdBuild { get; private set; }

        public int IdKiller { get; private set; }

        public string? Name { get; private set; }

        public int IdPerk1 { get; private set; }

        public int IdPerk2 { get; private set; }

        public int IdPerk3 { get; private set; }

        public int IdPerk4 { get; private set; }

        public int IdAddon1 { get; private set; }

        public int IdAddon2 { get; private set; }

        public static (KillerBuildDomain? KillerBuildDomain, string? Message) Create(int idBuild, int idKiller, string? name, int idPerk1, int idPerk2, int idPerk3, int idPerk4, int idAddon1, int idAddon2)
        {
            string message = string.Empty;

            var build = new KillerBuildDomain(idBuild, idKiller, name, idPerk1, idPerk2, idPerk3, idPerk4, idAddon1, idAddon2);

            return (build, message);
        }
    }
}