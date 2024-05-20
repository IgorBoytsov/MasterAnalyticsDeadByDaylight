using MasterAnalyticsDeadByDaylight.Command;
using MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel;
using Newtonsoft.Json.Serialization;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MasterAnalyticsDeadByDaylight.MVVM.ViewModel
{
    public class AddMatchWindowViewModel : BaseViewModel
    {

        #region Свойства для Убийцы    

        #endregion

        #region Свойства для Выжившего

        #endregion

        #region Свойства - Общие  

        #endregion

        #region Свойства SelectedItem для Убийц

        #endregion

        #region Свойства SelectedItem для Выживших

        #endregion

        #region Свойства SelectedItem - Общие

        #endregion

        public AddMatchWindowViewModel()
        {

        }

        #region Свойства

        public ObservableCollection<ImageInformation> CompressedImagesList { get; set; } = [];

        public ObservableCollection<string> NormalImagesList { get; set; } = [];

        public string Title { get; set; } = "Добавить запись";

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Команды

        private RelayCommand _loadImageCommand;
        public RelayCommand LoadImageCommand { get => _loadImageCommand ??= new(async obj => await UploadCompressedImages()); }

        #endregion

        #region Методы

        /// <summary>
        /// Загрузка сжатых изображений из папки "thumbnails", которая находится в основной папки с скриншотами игры. 
        /// </summary>
        /// <returns></returns>
        private async Task UploadCompressedImages()
        {
            string[] compressedImagePaths = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots\thumbnails");

            List<Task<ImageInformation>> tasks = new List<Task<ImageInformation>>();

            foreach (var path in compressedImagePaths)
            {
                tasks.Add(Task.Run(() => GetImageInformation(path)));
            }

            var imageInfoList = await Task.WhenAll(tasks);
            foreach (var item in imageInfoList)
            {
                CompressedImagesList.Add(item);
            }

            UploadNormalImage();
        }

        private Task<BitmapImage> RemoveHalfPixel(string path)
        {
            //Получить путь к изображению
            //Проверить является ли файл изображением
            //Превратить его в Bitmap
            Bitmap bitmap = new(path);
            //Сжать в меньший размер( в 2-4 раза)
            //Вернуть изображение в формате BitmapImage

            BitmapImage bitmapImage = new();

            return Task.Run(() => 
            {
                return bitmapImage;
            });
        }

        /// <summary>
        /// Получение информации о изображение.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ImageInformation GetImageInformation(string path)
        {
            try
            {   
                using (FileStream fs = File.OpenRead(path))
                {
                    return new ImageInformation
                    {
                        PathImage = path,
                        FileName = Path.GetFileName(path),
                        //Image = bitmap
                    };                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {path}. Ошибка: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Создание списка с изображениями нормального разрешение. Которые находятся в основной папке с скриншотами.
        /// </summary>
        private void UploadNormalImage()
        {
            string[] NormalImagePaths = Directory.GetFiles(@"D:\Steam\userdata\189964443\760\remote\381210\screenshots");

            foreach (var item in NormalImagePaths)
            {
                NormalImagesList.Add(item);
            }
        }

        #endregion
    }
}
