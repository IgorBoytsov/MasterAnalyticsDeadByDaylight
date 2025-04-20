namespace DBDAnalytics.Domain.DomainModels.DetailsModels
{
    public class DetailsMatchSurvivorDomain
    {
        private DetailsMatchSurvivorDomain()
        {

        }

        public int IdSurvivor { get; private set; }

        public int IdTypeDeath { get; private set; }

        public int IdPlatform { get; private set; }

        public int IdAssociation { get; private set; }

        public bool Bot { get; private set; }

        public bool Anonymous { get; private set; }

        public static DetailsMatchSurvivorDomain Create(
            int idSurvivor, int idTypeDeath, int idPlatform, int idAssociation,
            bool bot, bool anonymous)
        {
            var stats = new DetailsMatchSurvivorDomain
            {
                IdSurvivor = idSurvivor,
                IdTypeDeath = idTypeDeath,
                IdPlatform = idPlatform,
                IdAssociation = idAssociation,
                Bot = bot,
                Anonymous = anonymous,
            };

            return stats;
        }
    }
}