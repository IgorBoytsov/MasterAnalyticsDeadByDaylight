using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        public MapPage()
        {
            InitializeComponent();
            DataContext = new MapPageViewModel();
        }
    }
}
