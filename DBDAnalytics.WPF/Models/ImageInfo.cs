using System.Windows.Media.Imaging;

namespace DBDAnalytics.WPF.Models
{
    internal class ImageInfo
    {
        public string PathImage { get; set; }

        public byte[] ImageByte { get; set; }

        public string FileName { get; set; }

        public DateTime FileCreatedTime { get; set; }
    }
}