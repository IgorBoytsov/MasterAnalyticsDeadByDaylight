using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class SurvivorPerkCategoryDTO : BaseDTO<SurvivorPerkCategoryDTO>
    {
        public int IdSurvivorPerkCategory { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }
    }
}