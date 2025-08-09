using MasterAnalyticsDeadByDaylight.MVVM.View.Windows.ModalWindow;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Services.DialogService
{
    public class CustomDialogService : ICustomDialogService
    {
        public bool ShowMessage(string message, 
                                string messageDescription = "Ошибка", 
                                TypeMessage typeMessage = TypeMessage.Notification, 
                                MessageButtons messageButtons = MessageButtons.OKCancel)
        {
            var notificationWindow = new NotificationWindow(message, messageDescription, typeMessage, messageButtons);
            notificationWindow.ShowDialog();
            return notificationWindow.Result;
        }

        public MessageButtons ShowMessageButtons(string message, 
                                                 string messageDescription = "Ошибка", 
                                                 TypeMessage typeMessage = TypeMessage.Notification, 
                                                 MessageButtons messageButtons = MessageButtons.OKCancel)
        {
            var notificationWindow = new NotificationWindow(message, messageDescription, typeMessage, messageButtons);
            notificationWindow.ShowDialog();
            return notificationWindow.ResultButton;
        }
    }
}
