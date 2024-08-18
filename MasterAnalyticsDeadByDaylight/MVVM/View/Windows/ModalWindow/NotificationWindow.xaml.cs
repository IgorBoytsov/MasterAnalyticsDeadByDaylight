using MasterAnalyticsDeadByDaylight.MVVM.ViewModel.WindowsViewModels;
using MasterAnalyticsDeadByDaylight.Utils.Enum;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.ModalWindow
{
    /// <summary>
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {

        #region Обработчики кнопок на Header приложение

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
            }
        }

        #endregion

        public bool Result {  get; private set; }

        public MessageButtons ResultButton {  get; private set; }

        public NotificationWindow(string message, string messageDescription, TypeMessage typeMessage, MessageButtons messageButtons = MessageButtons.OKCancel)
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            MessageTextBlock.Text = message;
            MessageDescriptionTextBlock.Text = messageDescription;
            CheckMessageButtonsResult(messageButtons);
            CheckTypeMessage(typeMessage);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            ResultButton = MessageButtons.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            ResultButton = MessageButtons.Cancel;
            this.Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = true;
            ResultButton = MessageButtons.Yes;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = false;
            ResultButton = MessageButtons.No;
            this.Close();
        }

        private void CheckTypeMessage(TypeMessage message)
        {
            if (message == TypeMessage.Error)
            {
                TitleText.Text = "Ошибка";
                TitleText.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
                Header.Background = (Brush)new BrushConverter().ConvertFromString("#FF840000");
            }
            else if(message == TypeMessage.Notification)
            {
                TitleText.Text = "Информация";
                TitleText.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
                Header.Background = (Brush)new BrushConverter().ConvertFromString("#FF19450F");
            }
            else if (message == TypeMessage.Warning)
            {
                TitleText.Text = "Предупреждение!";
                TitleText.Foreground = (Brush)new BrushConverter().ConvertFromString("White");
                Header.Background = (Brush)new BrushConverter().ConvertFromString("#FF928F00");
            }
        }

        private void CheckMessageButtonsResult(MessageButtons messageButtons)
        {
            if (messageButtons == MessageButtons.OK)
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.Cancel)
            {
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.OKCancel)
            {
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.Yes)
            {
                OkButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.No)
            {
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.YesNo)
            {
                OkButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
            }
            else if (messageButtons == MessageButtons.YesNoCancel)
            {
                OkButton.Visibility = Visibility.Collapsed;
            }

        }
    }
}
