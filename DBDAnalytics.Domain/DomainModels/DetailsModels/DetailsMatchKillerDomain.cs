namespace DBDAnalytics.Domain.DomainModels.DetailsModels
{
    public class DetailsMatchKillerDomain
    {
        private DetailsMatchKillerDomain()
        {

        }

        public int IdKiller { get; private set; }

        public int? FirstAddonID { get; private set; }

        public int? SecondAddonID { get; private set; }

        public int? FirstPerkID { get; private set; }

        public int? SecondPerkID { get; private set; }

        public int? ThirdPerkID { get; private set; }

        public int? FourthPerkID { get; private set; }

        public int Score { get; private set; }

        public static DetailsMatchKillerDomain Create(
            int idKiller,
            int? idFirstAddon, int? idSecondAddon,
            int? firstPerkID, int? secondPerkID, int? thirdPerkID, int? fourthPerkID,
            int score)
        {
            var stats = new DetailsMatchKillerDomain
            {
                IdKiller = idKiller,
                FirstAddonID = idFirstAddon,
                SecondAddonID = idSecondAddon,
                FirstPerkID = firstPerkID,
                SecondPerkID = secondPerkID,
                ThirdPerkID = thirdPerkID,
                FourthPerkID = fourthPerkID,
                Score = score
            };

            return stats;
        }
    }
}