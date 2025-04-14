using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DBDAnalytics.Application.DTOs.BaseDTOs
{
    public abstract class BaseDTO<T> : INotifyPropertyChanged where T : BaseDTO<T>
    {
        protected BaseDTO() { }

        public T SetProperty(Action<T> setAction)
        {
            setAction((T)this);
            return (T)this;
        }

        #region Обобщенный метод создание DTO + пример использования

        //public static (T? Dto, string Message) Create<U>(Func<U, T> factoryMethod, 
        //                                                 U args, 
        //                                                 Func<U, string?> validation)
        //{
        //    string? message = validation(args);

        //    if (!string.IsNullOrWhiteSpace(message))
        //    {
        //        return (null, message);
        //    }

        //    var dto = factoryMethod(args);

        //    return (dto, string.Empty);
        //}

        //public static (GameModeDTO? GameModeDTO, string Message) Create(int idGameMode, string gameModeName, string? gameModeDescription)
        //{
        //    return BaseDTO<GameModeDTO>.Create(
        //        args => new GameModeDTO(args.idGameMode, args.gameModeName, args.gameModeDescription),
        //        (idGameMode, gameModeName, gameModeDescription),
        //        args =>
        //        {
        //            if (string.IsNullOrWhiteSpace(args.gameModeName))
        //            {
        //                return "Название режима не может быть пустым.";
        //            }

        //            return null;
        //        });
        //}

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}