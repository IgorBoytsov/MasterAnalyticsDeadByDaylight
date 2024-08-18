using MasterAnalyticsDeadByDaylight.Utils.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterAnalyticsDeadByDaylight.Services.DialogService
{
    interface IDialogService
    {
        bool ShowMessage(string message, string messageDescription = "Ошибка", TypeMessage typeMessage = TypeMessage.Notification, MessageButtons messageButtons = MessageButtons.OKCancel);
        MessageButtons ShowMessageButtons(string message, string messageDescription = "Ошибка", TypeMessage typeMessage = TypeMessage.Notification, MessageButtons messageButtons = MessageButtons.OKCancel);
    }
}
