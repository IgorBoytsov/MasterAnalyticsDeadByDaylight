using System.Windows.Controls;

namespace Shared.WPF.Navigations.Pages
{
    public interface IPageFactory
    {
        Page CreatePage();
    }
}