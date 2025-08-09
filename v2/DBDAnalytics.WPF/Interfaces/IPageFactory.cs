using DBDAnalytics.WPF.Enums;
using System.Windows.Controls;

namespace DBDAnalytics.WPF.Interfaces
{
    internal interface IPageFactory
    {
        Page CreatePage(object? parameter = null, TypeParameter typeParameter = TypeParameter.None);
    }
}