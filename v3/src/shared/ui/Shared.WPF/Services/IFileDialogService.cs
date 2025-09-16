namespace Shared.WPF.Services
{
    public interface IFileDialogService
    {
        string? OpenFileDialog(string title, string filter);
        IEnumerable<string>? OpenMultipleFilesDialog(string title, string filter);
        string? SaveFileDialog(string title, string filter, string defaultFileName = "");
        string? SelectFolderDialog(string title);
    }
}