using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using MasterAnalyticsDeadByDaylight.Services.NavigationService;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    internal class ShowDetailsMatchWindowViewModel : BaseViewModel, IUpdatable
    {
        private GameStatistic _match;
        public GameStatistic Match
        {
            get => _match;
            set
            {
                _match = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public ShowDetailsMatchWindowViewModel()
        {
            
        }

        public void Update(object value)
        {
            if (value is GameStatistic match)
            {
                Match = match;
                Title = $"Матч за {Match.DateTimeMatch.Value:D}, Режим : {Match.IdGameModeNavigation.GameModeName}, Ивент : {Match.IdGameEventNavigation.GameEventName}";
            }
        }
    }
}
