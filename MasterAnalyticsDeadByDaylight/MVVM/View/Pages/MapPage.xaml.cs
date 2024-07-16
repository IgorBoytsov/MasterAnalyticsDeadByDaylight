using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;
using System.Drawing;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        MapPageViewModel ViewModel { get; set; }

        public MapPage()
        {
            InitializeComponent();
            ViewModel = new MapPageViewModel();
            DataContext = ViewModel;
        }
    }
}
