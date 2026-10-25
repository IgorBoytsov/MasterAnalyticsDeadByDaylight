using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Base;

namespace DBDAnalytics.Client.WPF.ViewModels.Pages
{
    internal sealed class MatchesPageViewModel : BasePageViewModel, IAsyncInitializable, IUpdatable
    {
        /*--Инициализация---------------------------------------------------------------------------------*/

        public MatchesPageViewModel()
        {
            
        }

        Task IAsyncInitializable.InitializeAsync()
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        void IUpdatable.Update<TData>(TData value, TransmittingParameter parameter)
        {
            //throw new NotImplementedException();
        }

        /*--Коллекции-------------------------------------------------------------------------------------*/

        /*--Свойства--------------------------------------------------------------------------------------*/

        /*--Команды---------------------------------------------------------------------------------------*/

        /*--Методы----------------------------------------------------------------------------------------*/
    }
}