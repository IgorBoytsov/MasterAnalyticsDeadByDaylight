using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MasterAnalyticsDeadByDaylight.Utils.Helper
{
    public static class ImageHelper
    {
        public static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new())
            {
                image.Save(memoryStream, ImageFormat.Png);

                return memoryStream.ToArray();
            }
        }

        public static async Task<BitmapImage> RemoveHalfPixel(string path)
        {
            Bitmap bitmap = new(path);
            BitmapImage bitmapImage = new();

            Bitmap result = new Bitmap(bitmap.Width / 2, bitmap.Height / 2);
            await Task.Run(async () =>
            {
                try
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            bool isEvenX = x % 2 == 0;
                            bool isEvenY = y % 2 == 0;

                            if (isEvenX && isEvenY)
                            {
                                continue;
                            }

                            Color color = bitmap.GetPixel(x, y);
                            result.SetPixel(x / 2, y / 2, color);
                        }
                    }
                    bitmapImage = await ConvertBitmapToBitmapImageAsync(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            bitmap.Dispose();
            return bitmapImage;
        }

        public static async Task<BitmapImage> ConvertBitmapToBitmapImageAsync(Bitmap bitmap)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        bitmap.Save(memory, ImageFormat.Png);

                        memory.Seek(0, SeekOrigin.Begin); // Перемещаем указатель потока в начало

                        BitmapImage bitmapImage = new BitmapImage();

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

        public static async Task<BitmapImage> GetResultMatchImageAsync(string ImagePath)
        {
            Bitmap bitmap = new(ImagePath);

            bitmap = CropImage(bitmap, 0, 0, 1200, 1200);

            BitmapImage bitmapImage = new();

            return await Task.Run(() =>
            {
                return GetBitmapImage(bitmap);
            });
        }

        public static async Task<BitmapImage> GetStartMatchImageAsync(string ImagePath)
        {
            Bitmap bitmap = new(ImagePath);

            bitmap = CropImage(bitmap, 0, 900, 1100, 500);

            BitmapImage bitmapImage = new();

            return await Task.Run(() =>
            {
                return GetBitmapImage(bitmap);
            });
        }

        public static async Task<BitmapImage> GetEndMatchImageAsync(string ImagePath)
        {
            Bitmap bitmap = new(ImagePath);

            bitmap = CropImage(bitmap, 0, 250, 800, 1100);

            BitmapImage bitmapImage = new();

            return await Task.Run(() =>
            {
                return GetBitmapImage(bitmap);
            });
        }

        public static BitmapImage GetBitmapImage(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream memory = new())
                {
                    bitmap.Save(memory, ImageFormat.Png);

                    // Перемещаем указатель потока в начало
                    memory.Seek(0, SeekOrigin.Begin);

                    BitmapImage bitmapImage = new();

                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = memory;
                    bitmapImage.EndInit();

                    // Обеспечиваем доступ к свойствам BitmapImage на основном UI потоке
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

        public static Bitmap CropImage(Bitmap image, int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || x + width > image.Width || y + height > image.Height)
            {
                MessageBox.Show($"Координаты области обрезки находятся за пределами изображения.");
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
