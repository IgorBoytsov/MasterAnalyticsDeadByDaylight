using System.Windows.Media.Imaging;

namespace MasterAnalyticsDeadByDaylight.MVVM.Model.AppModel
{
    public class ImageInformation
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string PathImage {  get; set; }

        /// <summary>
        /// Сжатое изображение. Нужно для отображение в списке.
        /// </summary>
        public string ResizeImage { get; set; }
        
        /// <summary>
        /// Имя файла, без расширения
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Время создание файла
        /// </summary>
        public string FileCreatedTime { get; set; }
    }
}
