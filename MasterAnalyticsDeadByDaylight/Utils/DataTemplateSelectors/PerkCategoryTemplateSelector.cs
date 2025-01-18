using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using System.Windows;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Utils.DataTemplateSelectors
{
    public class PerkCategoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate KillerPerkCategoryTemplate { get; set; }
        public DataTemplate SurvivorPerkCategoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is KillerPerkCategory)
                return KillerPerkCategoryTemplate;
            if (item is SurvivorPerkCategory)
                return SurvivorPerkCategoryTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
