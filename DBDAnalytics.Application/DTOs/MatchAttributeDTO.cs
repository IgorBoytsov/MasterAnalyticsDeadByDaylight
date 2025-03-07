namespace DBDAnalytics.Application.DTOs
{
    public class MatchAttributeDTO
    {
        public int IdMatchAttribute { get;  set; }

        public string AttributeName { get;  set; } = null!;

        public string? AttributeDescription { get;  set; }

        public DateTime CreatedAt { get;  set; }

        public bool IsHide { get; set; }
    }
}