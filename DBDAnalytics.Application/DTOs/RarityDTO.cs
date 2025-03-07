using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class RarityDTO : BaseDTO<RarityDTO>
    {
        public int IdRarity { get; set; }

        public string RarityName { get; set; } = null!;
    }
}