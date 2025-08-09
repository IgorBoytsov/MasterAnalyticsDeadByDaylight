using DBDAnalytics.WPF.Enums;

namespace DBDAnalytics.WPF.Interfaces
{
    internal interface IUpdatable
    {
        void Update(object? parameter, TypeParameter typeParameter = TypeParameter.None);
    }
}
