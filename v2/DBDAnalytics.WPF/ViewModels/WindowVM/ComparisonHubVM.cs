using DBDAnalytics.Application.ApplicationModels.ComparisonModels;
using DBDAnalytics.WPF.Enums;
using DBDAnalytics.WPF.Interfaces;
using System.Collections.ObjectModel;

namespace DBDAnalytics.WPF.ViewModels.WindowVM
{
    internal class ComparisonHubVM : BaseVM, IUpdatable
    {
        public ComparisonHubVM()
        {
            
        }

        public void Update(object parameter, TypeParameter typeParameter = TypeParameter.None)
        {
            
        }

        /*--Свойства \ Коллекции--------------------------------------------------------------------------*/

        public ObservableCollection<ComparisonStats> Comparisons { get; set; } = [];

        private ComparisonStats _selectedComparison;
        public ComparisonStats SelectedComparison
        {
            get => _selectedComparison;
            set
            {
                _selectedComparison = value;
                OnPropertyChanged();
            }
        }
    }
}
