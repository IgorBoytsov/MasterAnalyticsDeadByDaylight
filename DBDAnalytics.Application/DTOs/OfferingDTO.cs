using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class OfferingDTO : BaseDTO<OfferingDTO>
    {
       public int IdOffering { get; set; }

        public int IdRole { get; set; }

        public int? IdCategory { get; set; }

        public int? IdRarity { get; set; }

        public string OfferingName { get; set; } = null!;

        public byte[]? OfferingImage { get; set; }

        public string? OfferingDescription { get; set; }
    }
}
