using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.AdminPanel.WPF.Resources.Attachable
{
    public static class ButtonHelper
    {
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.RegisterAttached(
                "IconTemplate",
                typeof(ControlTemplate),
                typeof(ButtonHelper),
                new PropertyMetadata(null));

        public static void SetIconTemplate(DependencyObject element, ControlTemplate value)
        {
            element.SetValue(IconTemplateProperty, value);
        }

        public static ControlTemplate GetIconTemplate(DependencyObject element)
            => (ControlTemplate)element.GetValue(IconTemplateProperty);
    }
}