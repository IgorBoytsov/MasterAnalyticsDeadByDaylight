using System.Drawing;

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
        public Bitmap ResizeImage { get; set; }
        
        /// <summary>
        /// Имя файла, без расширения
        /// </summary>
        public string FileName { get; set; }
    }
}
