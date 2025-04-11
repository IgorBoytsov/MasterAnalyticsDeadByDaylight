namespace DBDAnalytics.Domain.DomainModels.DetailsMatchView
{
    public class DetailsMatchSurvivorViewDomain
    {
        private DetailsMatchSurvivorViewDomain()
        {
            
        }

        /*Информация выжившего*/
        public byte[]? Image { get; private set; }

        public string? Name { get; private set; }

        public int Prestige { get; private set; }

        public double Score { get; private set; }

        public bool IsAnonymous { get; private set; }

        public bool IsBot { get; private set; }

        public string? PlayerAssociation { get; private set; }

        public string? Platform { get; private set; }

        public string? TypeDeath { get; private set; }

        /*Информация предмет - улучшения*/

        public byte[]? ItemImage { get; private set; }
        public string? ItemName { get; private set; }

        public byte[]? FirstItemAddonImage { get; private set; }
        public string? FirstItemAddonName { get; private set; }

        public byte[]? SecondItemAddonImage { get; private set; }
        public string? SecondItemAddonName { get; private set; }

        /*Информация перков*/

        public byte[]? FirstPerkImage { get; private set; }
        public string? FirstPerkName { get; private set; }

        public byte[]? SecondPerkImage { get; private set; }
        public string? SecondPerkName { get; private set; }

        public byte[]? ThirdPerkImage { get; private set; }
        public string? ThirdPerkName { get; private set; }

        public byte[]? FourthPerkImage { get; private set; }
        public string? FourthPerkName { get; private set; }

        /*Информация подношения*/

        public byte[]? OfferingImage { get; private set; }
        public string? OfferingName { get; private set; }

        public static DetailsMatchSurvivorViewDomain Create(
            byte[]? image, string? name, int prestige, int score, bool isAnonymous, bool isBot, string? playerAssociation, string? platform, string? typeDeath,
            byte[]? itemImage, string? itemName,
            byte[]? firstItemAddonImage, string? firstItemAddonName,
            byte[]? secondItemAddonImage, string? secondItemAddonName,
            byte[]? firstPerkImage, string? firstPerkName,
            byte[]? secondPerkImage, string? secondPerkName,
            byte[]? thirdPerkImage, string? thirdPerkName,
            byte[]? fourthPerkImage, string? fourthPerkName,
            byte[]? offeringImage, string? offeringPerkName)
        {
            return new DetailsMatchSurvivorViewDomain
            {
                Image = image,
                Name = name,
                Prestige = prestige,
                Score = score,
                IsAnonymous = isAnonymous,
                IsBot = isBot,
                PlayerAssociation = playerAssociation,
                Platform = platform,
                TypeDeath = typeDeath,

                ItemImage = itemImage,
                ItemName = itemName,

                FirstItemAddonImage = firstItemAddonImage,
                FirstItemAddonName = firstItemAddonName,

                SecondItemAddonImage = secondItemAddonImage,
                SecondItemAddonName = secondItemAddonName,

                FirstPerkImage = firstPerkImage,
                FirstPerkName = firstPerkName,

                SecondPerkImage = secondPerkImage,
                SecondPerkName = secondPerkName,

                ThirdPerkImage = thirdPerkImage,
                ThirdPerkName = thirdPerkName,

                FourthPerkImage = fourthPerkImage,
                FourthPerkName = fourthPerkName,  

                OfferingImage = offeringImage,
                OfferingName = offeringPerkName,
            };
        }
    }
}