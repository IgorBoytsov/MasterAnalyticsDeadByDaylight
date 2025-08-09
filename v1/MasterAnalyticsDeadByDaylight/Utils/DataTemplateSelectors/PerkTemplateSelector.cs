using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using System.Windows;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Utils.DataTemplateSelectors
{
    class PerkTemplateSelector : DataTemplateSelector
    {
        public DataTemplate KillerPerkTemplate { get; set; }
        public DataTemplate SurvivorPerkTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is KillerPerk)
                return KillerPerkTemplate;
            if (item is SurvivorPerk)
                return SurvivorPerkTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
