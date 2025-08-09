using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.Services.NavigationService.PageNavigation
{
    public interface IPageNavigationService
    {
        public void SetFrame(Frame frame);
        void NavigateTo(string pageName, object parameter = null, bool onlyUpdate = false);
    }
}
