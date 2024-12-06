using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    internal class ShowDetailsMatchWindowViewModel : BaseViewModel, IUpdatable
    {
        private GameStatistic _match;
        public GameStatistic Math
        {
            get => _match;
            set
            {
                _match = value;
                OnPropertyChanged();
            }
        }

        private readonly IServiceProvider _serviceProvider;

        public ShowDetailsMatchWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }
    }
}
