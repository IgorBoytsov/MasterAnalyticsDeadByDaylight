using MasterAnalyticsDeadByDaylight.Services.NavigationService;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels
{
    public class HowToUseWindowViewModel : BaseViewModel, IUpdatable
    {
        private readonly IServiceProvider _serviceProvider;

        public HowToUseWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Update(object value)
        {
            throw new NotImplementedException();
        }
    }
}
