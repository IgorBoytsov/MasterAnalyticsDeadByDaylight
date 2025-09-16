using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;

namespace Shared.WPF.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string? OpenFileDialog(string title, string filter)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = title,
                    Filter = filter,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false
                };

                if (dialog.ShowDialog() == true)
                    return dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public IEnumerable<string>? OpenMultipleFilesDialog(string title, string filter)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Title = title,
                    Filter = filter,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = true
                };

                if (dialog.ShowDialog() == true)
                    return dialog.FileNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии файлов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public string? SaveFileDialog(string title, string filter, string defaultFileName = "")
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = title,
                    Filter = filter,
                    FileName = defaultFileName
                };

                if (dialog.ShowDialog() == true)
                    return dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        public string? SelectFolderDialog(string title)
        {
            try
            {
                var dialog = new CommonOpenFileDialog
                {
                    Title = title,
                    IsFolderPicker = true
                };

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    return dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выборе папки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }
    }
}