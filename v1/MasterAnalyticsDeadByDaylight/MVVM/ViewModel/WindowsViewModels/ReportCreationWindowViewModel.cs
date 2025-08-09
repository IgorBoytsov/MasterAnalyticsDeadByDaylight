using MasterAnalyticsDeadByDaylight.Services.NavigationService;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class ReportCreationWindowViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportCreationWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }
    }
}
