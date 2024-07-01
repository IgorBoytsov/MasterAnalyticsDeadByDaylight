using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.PagesViewModels;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для KillerPage.xaml
    /// </summary>
    public partial class KillerPage : Page
    {
        public KillerPage()
        {
            InitializeComponent();
            DataContext = new KillerPageViewModel();
        }
    }
}
