using DBDAnalytics.WPF.Enums;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Interfaces
{
    internal interface IPageNavigationService
    {
        void Close(PageName pageName, FrameName frameName);
        void Navigate(PageName pageName, FrameName frameName, object parameter = null, TypeParameter typeParameter = TypeParameter.None);
        void RegisterFrame(FrameName frameName, Frame frame);
        void TransmittingValue(PageName pageName, FrameName frameName, object parameter = null, TypeParameter typeParameter = TypeParameter.None);
    }
}