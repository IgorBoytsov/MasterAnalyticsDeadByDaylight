namespace DBDAnalytics.Domain.DomainModels.DetailsModels
{
    public class DetailsMatchSurvivorDomain
    {
        private DetailsMatchSurvivorDomain()
        {

        }

        public int IdSurvivor { get; private set; }

        public int? IdFirstPerk { get; private set; }

        public int? IdSecondPerk { get; private set; }

        public int? IdThirdPerk { get; private set; }

        public int? IdFourthPerk { get; private set; }

        public int IdTypeDeath { get; private set; }

        public int IdPlatform { get; private set; }

        public int IdAssociation { get; private set; }

        public bool Bot { get; private set; }

        public bool Anonymous { get; private set; }

        public static DetailsMatchSurvivorDomain Create(
            int idSurvivor, int? idFirstPerk, int? idSecondPerk, int? idThirdPerk, int? idFourthPerk, 
            int idTypeDeath, int idPlatform, int idAssociation,bool bot, bool anonymous)
        {
            var stats = new DetailsMatchSurvivorDomain
            {
                IdSurvivor = idSurvivor,

                IdFirstPerk = idFirstPerk,
                IdSecondPerk = idSecondPerk,
                IdThirdPerk = idThirdPerk,
                IdFourthPerk = idFourthPerk,

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