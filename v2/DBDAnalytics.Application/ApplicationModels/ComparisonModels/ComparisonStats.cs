using DBDAnalytics.Application.Enums;
using System.Collections.ObjectModel;

namespace DBDAnalytics.Application.ApplicationModels.ComparisonModels
{
    public class ComparisonStats
    {
        public string? Name { get; set; }

        public ObservableCollection<object> Comparisons { get; set; } = [];
    }
}
