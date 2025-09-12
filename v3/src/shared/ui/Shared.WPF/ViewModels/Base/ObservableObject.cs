using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.WPF.ViewModels.Base
{
    public abstract class ObservableObject<TModel>(TModel model) : BaseViewModel where TModel : class
    {   
        protected TModel _model = model ?? throw new ArgumentNullException(nameof(model));

        /// <summary>
        /// Создает новый экземпляр модели на основе текущих, отредактированных данных из ViewModel.
        /// </summary>
        public abstract TModel ToModel();

        /// <summary>
        /// Обновляет внутреннюю базовую модель.
        /// </summary>
        /// <param name="newModel">Новый, подтвержденный экземпляр модели.</param>
        public virtual void CommitChanges(TModel newModel)
        {
            _model = newModel ?? throw new ArgumentNullException(nameof(newModel));
            OnPropertyChanged(string.Empty);
        }

        /// <summary>
        /// Отменяет все изменения, возвращая значения из последней сохраненной модели.
        /// </summary>
        public abstract void RevertChanges();
    }
}