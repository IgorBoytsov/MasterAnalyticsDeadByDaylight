using System.Diagnostics;

namespace Shared.WPF.Commands
{
    public class RelayCommandAsync<T> : CommandBase
    {
        private readonly Func<T, Task> _executeAsync;
        private readonly Predicate<T> _canExecute;
        private volatile bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommandAsync(Func<T, Task> executeAsync) : this(executeAsync, null) { }

        public RelayCommandAsync(Func<T, Task> executeAsync, Predicate<T> canExecute)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (IsExecuting)
                return false;

            if (!TryConvertParameter<T>(parameter, out T typedParameter))
                if (typeof(T) != typeof(object) && parameter != null && !(parameter is T))
                    return false;

            return _canExecute == null || _canExecute(typedParameter);
        }

        public override async void Execute(object parameter)
        {
            if (!TryConvertParameter<T>(parameter, out T typedParameter))
            {
                if (typeof(T) != typeof(object) && parameter != null && !(parameter is T))
                {
                    Debug.WriteLine($"AsyncRelayCommand: Неверный тип параметра '{parameter?.GetType().Name}' для команды с типом параметра '{typeof(T).Name}'. Выполнение отменено.");
                    return;
                }
            }

            if (!CanExecute(parameter))
                return;

            IsExecuting = true;

            try
            {
                await _executeAsync(typedParameter);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка: {ex.Message}. Выполнение отменено.");
            }
            finally
            {
                IsExecuting = false;
            }
        }
    }

    public class RelayCommandAsync : RelayCommandAsync<object>
    {
        public RelayCommandAsync(Func<Task> executeAsync) : base(async param => await executeAsync(), null)
            => ArgumentNullException.ThrowIfNull(executeAsync);

        public RelayCommandAsync(Func<Task> executeAsync, Func<bool> canExecute) : base(async param => await executeAsync(), param => canExecute())
            => ArgumentNullException.ThrowIfNull(executeAsync);
    }
}