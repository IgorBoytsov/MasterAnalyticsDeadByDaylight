namespace DBDAnalytics.Application.ApplicationModels.ComparisonModels
{
    public class BaseComparison
    {
        public string Name { get; set; } = null!;

        public byte[]? Image { get; set; }

        public int CountMatch { get; set; }

        public double PickRate { get; set; }

        public double WinRate { get; set; }
    }
}
