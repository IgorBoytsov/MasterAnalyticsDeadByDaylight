using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class KillerDTO : BaseDTO<KillerDTO>
    {
        public int IdKiller { get; set; }

        public string KillerName { get; set; } = null!;

        public byte[]? KillerImage { get; set; }

        public byte[]? KillerAbilityImage { get; set; }
    }
}
