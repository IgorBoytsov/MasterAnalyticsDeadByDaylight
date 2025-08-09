using DBDAnalytics.Application.DTOs.BaseDTOs;

namespace DBDAnalytics.Application.DTOs
{
    public class PatchDTO : BaseDTO<PatchDTO>
    {
        public int IdPatch { get; set; }

        public string PatchNumber { get; set; } = null!;

        public DateOnly PatchDateRelease { get; set; }

        public string? Description { get; set; }
    }
}