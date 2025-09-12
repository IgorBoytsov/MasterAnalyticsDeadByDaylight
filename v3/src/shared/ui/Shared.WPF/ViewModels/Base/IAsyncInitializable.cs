namespace Shared.WPF.ViewModels.Base
{
    public interface IAsyncInitializable
    {
        Task InitializeAsync();
    }
}