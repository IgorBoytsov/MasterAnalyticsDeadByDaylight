namespace DBDAnalytics.Domain.DomainModels.DetailsMatchView
{
    public class DetailsMatchKillerViewDomain
    {
        private DetailsMatchKillerViewDomain()
        {
            
        }

        /*Информация киллера*/
        public byte[]? Image { get; private set; }

        public byte[]? Ability { get; private set; }

        public string? Name { get; private set; }

        public int Prestige { get; private set; }

        public double Score { get; private set; }

        public bool IsAnonymous { get; private set; }

        public bool IsBot { get; private set; }

        public string? PlayerAssociation { get; private set; }

        public string? Platform { get; private set; }

        /*Информация улучшений*/

        public byte[]? FirstAddonImage { get; private set; }
        public string? FirstAddonName { get; private set; }

        public byte[]? SecondAddonImage { get; private set; }
        public string? SecondAddonName { get; private set; }

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

        public static DetailsMatchKillerViewDomain Create(
            byte[]? image, byte[]? ability, string? name, int prestige, int score, bool isAnonymous, bool isBot, string? playerAssociation, string? platform,
            byte[]? firstAddonImage, string? firstAddonName,
            byte[]? secondAddonImage, string? secondAddonName,
            byte[]? firstPerkImage, string? firstPerkName,
            byte[]? secondPerkImage, string? secondPerkName,
            byte[]? thirdPerkImage, string? thirdPerkName,
            byte[]? fourthPerkImage, string? fourthPerkName,
            byte[]? offeringImage, string? offeringName)
        {
            return new DetailsMatchKillerViewDomain
            {
                Image = image,
                Ability = ability,
                Name = name,
                Prestige = prestige,
                Score = score,
                IsAnonymous = isAnonymous,
                IsBot = isBot,
                PlayerAssociation = playerAssociation,
                Platform = platform,

                FirstAddonImage = firstAddonImage,
                FirstAddonName = firstAddonName,

                SecondAddonImage = secondAddonImage,
                SecondAddonName = secondAddonName,

                FirstPerkImage = firstPerkImage,
                FirstPerkName = firstPerkName,

                SecondPerkImage = secondPerkImage,
                SecondPerkName = secondPerkName,

                ThirdPerkImage = thirdPerkImage,
                ThirdPerkName = thirdPerkName,

                FourthPerkImage = fourthPerkImage,
                FourthPerkName = fourthPerkName,

                OfferingImage = offeringImage,
                OfferingName = offeringName
            };
        }
    }
}