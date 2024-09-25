using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Services.DialogService
{
    public interface ICustomDialogService
    {
        bool ShowMessage(string message, string messageDescription = "Ошибка", TypeMessage typeMessage = TypeMessage.Notification, MessageButtons messageButtons = MessageButtons.OKCancel);
        MessageButtons ShowMessageButtons(string message, string messageDescription = "Ошибка", TypeMessage typeMessage = TypeMessage.Notification, MessageButtons messageButtons = MessageButtons.OKCancel);
    }
}
