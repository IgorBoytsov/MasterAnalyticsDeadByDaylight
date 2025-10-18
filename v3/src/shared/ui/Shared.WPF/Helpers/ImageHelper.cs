﻿using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Shared.WPF.Helpers
{
    public static class ImageHelper
    {
        public static ImageSource? ImageFromFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static readonly HttpClient _httpClient = new();

        public static async Task<ImageSource?> ImageFromUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                Debug.WriteLine("[ImageUtils] URL пуст или состоит из пробелов.");
                return null;
            }

            try
            {
                return await Task.Run(async () =>
                {
                    var imageBytes = await _httpClient.GetByteArrayAsync(url);
                    using var memoryStream = new MemoryStream(imageBytes);
                    var bitmap = new BitmapImage();

                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = memoryStream;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    return (ImageSource)bitmap;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ImageUtils] Ошибка при загрузке или создании изображения по URL '{url}': {ex.Message}");
                return null;
            }
        }

        public static byte[]? ToBytes(ImageSource? imageSource)
        {
            if (imageSource is null)
                return null;

            if (imageSource is not BitmapSource bitmapSource)
                return null;

            using var stream = new MemoryStream();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(stream);
            return stream.ToArray();
        }

        public static BitmapImage ToBitmapImageFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using var stream = new MemoryStream(imageBytes);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        public static BitmapImage ToBitmapImageFromBitmap(Bitmap bitmap)
        {
            try
            {
                using MemoryStream memory = new();

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
            catch (Exception ex)
            {
                MessageBox.Show($"Не получилось от рендерить изображение \n {ex.Message}");
                return null;
            }
        }

        public static Bitmap CropImage(string path, int x, int y, int width, int height)
        {
            using var image = new Bitmap(path);

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