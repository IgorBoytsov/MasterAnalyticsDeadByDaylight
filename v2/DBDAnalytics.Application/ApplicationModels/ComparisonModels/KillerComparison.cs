namespace DBDAnalytics.Application.ApplicationModels.ComparisonModels
{
    public class KillerComparison : BaseComparison
    {
        public int CountMatchWin { get; set; }
        public int CountMatchDraw { get; set; }
        public int CountMatchLose { get; set; }
    }
}
