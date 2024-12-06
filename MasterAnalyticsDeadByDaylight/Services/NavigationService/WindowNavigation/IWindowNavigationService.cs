namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.WindowNavigation
{
    public interface IWindowNavigationService
    {
        void OpenWindow(string windowName, object parameter = null);
    }
}
