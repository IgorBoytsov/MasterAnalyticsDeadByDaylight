using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    class AddAdditionalDataWindowViewModel : BaseViewModel
    {
        public List<string> GameMode { get; set; } = ["Обычный", "Хаотичное перемешивание"];

        public AddAdditionalDataWindowViewModel() 
        { 
        
        }

    }
}
