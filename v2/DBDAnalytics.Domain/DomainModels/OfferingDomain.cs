namespace DBDAnalytics.Domain.DomainModels
{
    public class OfferingDomain
    {
        private OfferingDomain(int idOffering, int idRole, int? idCategory, int? idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            IdOffering = idOffering;
            IdRole = idRole;
            IdCategory = idCategory;
            IdRarity = idRarity;
            OfferingName = offeringName;
            OfferingImage = offeringImage;
            OfferingDescription = offeringDescription;
        }
        public int IdOffering { get; private set; }

        public int IdRole { get; private set; }

        public int? IdCategory { get; private set; }

        public int? IdRarity { get; private set; }

        public string OfferingName { get; private set; } = null!;

        public byte[]? OfferingImage { get; private set; }

        public string? OfferingDescription { get; private set; }

        public static (OfferingDomain? OfferingDomain, string Message) Create(int idOffering, int idRole, int? idCategory, int? idRarity, string offeringName, byte[]? offeringImage, string? offeringDescription)
        {
            string message = string.Empty;
            const int MaxOfferingNameLength = 100;
            const int MaxOfferingDescriptionLength = 1000;

            if (idRole < 0)
            {
                return (null, "Вы не указали роль, чьё подношение добавляете.");
            }

            if (idCategory < 0)
            {
                return (null, "Вы не указали категорию подношение.");
            }

            if (idRarity < 0)
            {
                return (null, "Вы не указали редкость подношение.");
            }

            if (string.IsNullOrWhiteSpace(offeringName))
            {
                return (null, "Укажите название подношению");
            } 
            
            if (offeringName.Length > MaxOfferingNameLength)
            {
                return (null, $"Максимально допустимая длинна названия - {MaxOfferingNameLength}");
            } 
            
            if (offeringDescription?.Length > MaxOfferingDescriptionLength)
            {
                return (null, $"Максимально допустимая длинна описания - {MaxOfferingDescriptionLength}");
            }

            var offering = new OfferingDomain(idOffering, idRole, idCategory, idRarity, offeringName, offeringImage, offeringDescription);

            return (offering, message);
        }
    }
}