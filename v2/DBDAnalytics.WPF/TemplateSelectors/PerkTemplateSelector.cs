using DBDAnalytics.Application.DTOs;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.TemplateSelectors
{
    internal class PerkTemplateSelector : DataTemplateSelector
    {
        public DataTemplate KillerPerkTemplate { get; set; }
        public DataTemplate SurvivorPerkTemplate { get; set; }
        public DataTemplate KillerCategoryTemplate { get; set; }
        public DataTemplate SurvivorCategoryTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is KillerPerkDTO)
            {
                return KillerPerkTemplate;
            }
            else if (item is SurvivorPerkDTO)
            {
                return SurvivorPerkTemplate;
            }
            else if (item is KillerPerkCategoryDTO)
            {
                return KillerCategoryTemplate ?? DefaultTemplate;
            }
            else if (item is SurvivorPerkCategoryDTO)
            {
                return SurvivorCategoryTemplate ?? DefaultTemplate;
            }

            return DefaultTemplate;
        }
    }
}