using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace MasterAnalyticsDeadByDaylight.MVVM.View.Windows.AppWindow
{
    /// <summary>
    /// Логика взаимодействия для DetailedMatchStatisticsWindow.xaml
    /// </summary>
    public partial class DetailedMatchStatisticsWindow : Window
    {
        public DetailedMatchStatisticsWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
        }

        private void survBTN_Click(object sender, RoutedEventArgs e)
        {
            SaveListViewToImage(survList, "E:\\survPerk.png");
        }

        private void killerBTN_Click(object sender, RoutedEventArgs e)
        {
            SaveListViewToImage(survList, "E:\\killerPerk.png");
        }

        public static void SaveListViewToImage(ListView listView, string filePath)
        {
            // Сохраняем исходные размеры ListView
            double originalWidth = listView.Width;
            double originalHeight = listView.Height;

            // Измеряем и изменяем размер ListView, чтобы вместить все элементы
            listView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            listView.Arrange(new Rect(listView.DesiredSize));

            // Устанавливаем новые размеры ListView
            listView.Width = listView.DesiredSize.Width;
            listView.Height = listView.DesiredSize.Height;

            // Рендерим ListView в изображение
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
                (int)listView.ActualWidth,
                (int)listView.ActualHeight,
                96, 96, PixelFormats.Pbgra32);

            renderTargetBitmap.Render(listView);

            // Кодируем изображение в PNG
            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            // Сохраняем изображение в файл
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                pngImage.Save(fileStream);
            }

            // Восстанавливаем исходные размеры ListView
            listView.Width = originalWidth;
            listView.Height = originalHeight;
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

       
    }
}
