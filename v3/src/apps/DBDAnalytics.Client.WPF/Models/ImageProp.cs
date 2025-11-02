using System.Windows.Media;

namespace DBDAnalytics.Client.WPF.Models
{
    public sealed class ImageProp
    {
        public string Path { get; set; } = null!;
        public ImageSource Image { get; set; } = null!;
    }
}