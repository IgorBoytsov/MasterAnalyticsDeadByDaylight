using MasterAnalyticsDeadByDaylight.Services.DialogService;
using MasterAnalyticsDeadByDaylight.Utils.Enum;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public static class MessageHelper
    {
        private static readonly string MessageExistsRecord = "Эта запись уже имеется, либо вы ничего не написали";
        private static readonly string MessageDescription = "Ошибка добавления";

        public static ICustomDialogService _dialogService = new CustomDialogService();

        public static readonly Action MessageExist = () => _dialogService.ShowMessage(MessageExistsRecord, MessageDescription, TypeMessage.Warning);
       
        public static readonly Action PrestigeMessage = () => _dialogService.ShowMessage("Престиже меньше 0 и выше 100 не бывает!", "Не верное значение", TypeMessage.Warning);
        
        /// <summary>
        /// Сообщение для предупреждение о изменение записи.
        /// <para>arg1 Текущее значение свойства.</para>
        /// <para>arg2 Новое значение свойства.</para>
        /// <para>arg3 Текущее значение свойства #2, в данном случае описание.</para>
        /// <para>arg4 Новое значение свойства #2, в данном случае описание.</para>
        /// </summary>
        /// <returns> Результат нажатой кнопки из перечисление MessageButtons </returns>
        public static readonly Func<string, string, string, string, MessageButtons> MessageUpdate = (CurrentName, ChangedName, CurrentDescription, ChangedDescription) =>
        _dialogService.ShowMessageButtons($"Вы точно хотите обновить ее? Если да, то будет произведена замена с «{CurrentName}» на «{ChangedName}» и «{CurrentDescription}» на «{ChangedDescription}»",
                                          $"Надпись с именем «{CurrentName}» уже существует.",
                                          TypeMessage.Notification, MessageButtons.YesNoCancel);
        /// <summary>
        /// Передать название элемента, который будет удален.
        /// </summary>
        public static readonly Func<string, MessageButtons> MessageDelete = (ItemName) =>
        _dialogService.ShowMessageButtons($"Вы точно хотите удалить «{ItemName}»? Это может привести к удалению связанных записей!",
                                          $"Предупреждение об удаление.",
                                          TypeMessage.Warning, MessageButtons.YesNo);
    }
}
