namespace DBDAnalytics.Application.DTOs.DetailsViewDTOs
{
    public class DetailsMatchSurvivorViewDTO
    {        
        /*Информация выжившего*/
        public byte[]? Image { get; set; }

        public string? Name { get; set; }

        public int Prestige { get; set; }

        public double Score { get; set; }

        public bool IsAnonymous { get; set; }

        public bool IsBot { get; set; }

        public string? PlayerAssociation { get; set; }

        public string? Platform { get; set; }

        public string? TypeDeath { get; set; }

        /*Информация предмет - улучшения*/
        public byte[]? ItemImage { get; set; }
        public string? ItemName { get; set; }

        public byte[]? FirstItemAddonImage { get; set; }
        public string? FirstItemAddonName { get; set; }

        public byte[]? SecondItemAddonImage { get; set; }
        public string? SecondItemAddonName { get; set; }

        /*Информация перков*/
        public byte[]? FirstPerkImage { get; set; }
        public string? FirstPerkName { get; set; }

        public byte[]? SecondPerkImage { get; set; }
        public string? SecondPerkName { get; set; }

        public byte[]? ThirdPerkImage { get; set; }
        public string? ThirdPerkName { get; set; }

        public byte[]? FourthPerkImage { get; set; }
        public string? FourthPerkName { get; set; }

        /*Информация подношения*/
        public byte[]? OfferingImage { get; set; }
        public string? OfferingName { get; set; }
    }
}