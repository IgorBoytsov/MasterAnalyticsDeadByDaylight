using Shared.WPF.Enums;

namespace Shared.WPF.ViewModels.Base
{
    public interface IUpdatable
    {
        void Update<TData>(TData value, TransmittingParameter parameter);
    }
}