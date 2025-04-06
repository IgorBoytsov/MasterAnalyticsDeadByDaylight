using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DBDAnalytics.WPF.Helpers
{
    public static class ImageHelper
    {
        public static byte[] ImageToByteArray(string imagePath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                using (MemoryStream memoryStream = new())
                {
                    image.Save(memoryStream, ImageFormat.Png);

                    return memoryStream.ToArray();
                }
            }
        }

        public static async Task<byte[]> ImageToByteArrayAsync(string imagePath)
        {
            return await Task.Run(() => 
            {
                using (var image = Image.FromFile(imagePath))
                {
                    using (MemoryStream memoryStream = new())
                    {
                        image.Save(memoryStream, ImageFormat.Png);

                        return memoryStream.ToArray();
                    }
                }
            });
        }

        public static byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            if (bitmapImage == null)
            {
                throw new ArgumentNullException(nameof(bitmapImage), "BitmapImage не может быть null.");
            }

            using (MemoryStream memoryStream = new())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public static async Task<BitmapImage> ConvertBitmapToBitmapImageAsync(Bitmap bitmap)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (MemoryStream memory = new())
                    {
                        bitmap.Save(memory, ImageFormat.Png);

                        memory.Seek(0, SeekOrigin.Begin); // Перемещаем указатель потока в начало

                        BitmapImage bitmapImage = new();

                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memory;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze(); // Обеспечиваем доступ к свойствам BitmapImage на основном UI потоке

                        return bitmapImage;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не получилось от рендерить изображение \n {ex.Message}");
                    return null;
                }
            });
        }

        public static BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream memory = new())
                {
                    bitmap.Save(memory, ImageFormat.Png);

                    memory.Seek(0, SeekOrigin.Begin);

                    BitmapImage bitmapImage = new();

                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memory;
                    bitmapImage.EndInit();

                    bitmapImage.Freeze();

                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не получилось от рендерить изображение \n {ex.Message}");
                return null;
            }
        }

        public static Bitmap CropImage(string path, int x, int y, int width, int height)
        {
            using (var image = new Bitmap(path))
            {
                if (x < 0 || y < 0 || x + width > image.Width || y + height > image.Height)
                {
                    throw new Exception($"Координаты области обрезки находятся за пределами изображения.");
                }

                Bitmap croppedImage = new(width, height);

                using (Graphics g = Graphics.FromImage(croppedImage))
                {
                    g.DrawImage(image, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                }
                return croppedImage;
            }
        }

    }
}
