namespace DBDAnalytics.Domain.DomainModels
{
    public class SurvivorBuildDomain
    {
        private SurvivorBuildDomain(int idBuild, string? name, int idPerk1, int idPerk2, int idPerk3, int idPerk4, int idItem, int idAddonItem1, int idAddonItem2)
        {
            IdBuild = idBuild;
            Name = name;
            IdPerk1 = idPerk1;
            IdPerk2 = idPerk2;
            IdPerk3 = idPerk3;
            IdPerk4 = idPerk4;
            IdItem = idItem;
            IdAddonItem1 = idAddonItem1;
            IdAddonItem2 = idAddonItem2;
        }

        public int IdBuild { get; private set; }

        public string? Name { get; private set; }

        public int IdPerk1 { get; private set; }

        public int IdPerk2 { get; private set; }

        public int IdPerk3 { get; private set; }

        public int IdPerk4 { get; private set; }

        public int IdItem { get; private set; }

        public int IdAddonItem1 { get; private set; }

        public int IdAddonItem2 { get; private set; }

        public static (SurvivorBuildDomain? SurvivorBuildDomain, string? Message) Create(int idBuild, string? name, int idPerk1, int idPerk2, int idPerk3, int idPerk4, int idItem, int idAddonItem1, int idAddonItem2)
        {
            string message = string.Empty;

            var build = new SurvivorBuildDomain(idBuild, name, idPerk1, idPerk2, idPerk3, idPerk4, idItem, idAddonItem1, idAddonItem2);

            return (build, message);
        }
    }
}
