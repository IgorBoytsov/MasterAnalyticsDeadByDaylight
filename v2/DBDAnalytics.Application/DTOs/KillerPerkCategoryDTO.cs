using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerPerkCategoryDTO : BaseDTO<KillerPerkCategoryDTO>
    {
        public int IdKillerPerkCategory { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }
    }
}