using DBDAnalytics.WPF.Enums;

namespace DBDAnalytics.WPF.Interfaces
{
    internal interface IWindowNavigationService
    {
        void CloseWindow(WindowName windowName);
        void MaximizeWindow(WindowName windowName);
        void MinimizeWindow(WindowName windowName);
        void OpenWindow(WindowName windowName, object parameter = null, TypeParameter typeParameter = TypeParameter.None, bool IsOpenDialog = false);
        void RestoreWindow(WindowName windowName);
        void ShutDownApp();
        void TransmittingValue(WindowName windowName, object parameter = null, TypeParameter typeParameter = TypeParameter.None);
    }
}