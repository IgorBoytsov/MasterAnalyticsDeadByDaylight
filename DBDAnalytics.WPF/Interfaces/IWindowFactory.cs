using DBDAnalytics.WPF.Enums;
using System.Windows;

namespace DBDAnalytics.WPF.Interfaces
{
    internal interface IWindowFactory
    {
        Window CreateWindow(object? parameter = null, TypeParameter typeParameter = TypeParameter.None);
    }
}
