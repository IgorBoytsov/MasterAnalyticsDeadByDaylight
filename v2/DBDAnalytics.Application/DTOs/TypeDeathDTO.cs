using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class TypeDeathDTO : BaseDTO<TypeDeathDTO>
    {
        public int IdTypeDeath { get; set; }

        public string TypeDeathName { get; set; } = null!;

        public string? TypeDeathDescription { get; set; }
    }
}
