using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MatchPage.xaml
    /// </summary>
    public partial class MatchPage : Page
    {
        public MatchPage()
        {
            InitializeComponent();
            DataContext = new MatchPageViewModel();
        }
    }
}
