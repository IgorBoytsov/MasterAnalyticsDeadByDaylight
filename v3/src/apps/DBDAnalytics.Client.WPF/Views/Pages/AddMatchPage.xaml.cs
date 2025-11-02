using DBDAnalytics.Client.WPF.ViewModels.Pages;
using System.Windows;
using System.Windows.Controls;

namespace DBDAnalytics.Client.WPF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddMatchPage.xaml
    /// </summary>
    public partial class AddMatchPage : Page
    {
        private AddMatchPageViewModel ViewModel => DataContext as AddMatchPageViewModel;

        public AddMatchPage()
        {
            InitializeComponent();
            this.SizeChanged += OnParentSizeChanged;
        }

        private void OnParentSizeChanged(object sender, SizeChangedEventArgs e) => UpdateImagesPopupSize();

        private void UpdateImagesPopupSize()
        {
            if (ViewModel?.ImagesPopup != null)
                ViewModel.ImagesPopup.UpdatePopupSize(this.ActualHeight);
        }
    }
}