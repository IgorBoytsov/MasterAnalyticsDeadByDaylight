using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SurvivorPage.xaml
    /// </summary>
    public partial class SurvivorPage : Page
    {
        public SurvivorPage()
        {
            InitializeComponent();
            DataContext = new SurvivorPageViewModel();
        }
    }
}
