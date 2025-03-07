using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class OfferingCategoryDTO : BaseDTO<OfferingCategoryDTO>
    {
        public int IdCategory { get; set; }

        public string CategoryName { get; set; } = null!;
    }
}