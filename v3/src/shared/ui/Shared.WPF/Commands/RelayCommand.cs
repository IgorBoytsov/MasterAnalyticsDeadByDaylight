namespace Shared.WPF.Commands
{
    public class RelayCommand<T> : CommandBase
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter)
        {
            if (!TryConvertParameter<T>(parameter, out T typedParameter))
                if (typeof(T) != typeof(object) && parameter != null && !(parameter is T))
                    return false;

            return _canExecute == null || _canExecute(typedParameter);
        }

        public override void Execute(object parameter)
        {
            if (!TryConvertParameter<T>(parameter, out T typedParameter))
                if (typeof(T) != typeof(object) && parameter != null && !(parameter is T))
                    throw new ArgumentException($"Параметр имеет тип {parameter?.GetType().Name}, но ожидался совместимый с {typeof(T).Name}.", nameof(parameter));

            if (CanExecute(parameter))
                _execute(typedParameter);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute) : base(param => execute(), null)
            => ArgumentNullException.ThrowIfNull(execute);

        public RelayCommand(Action execute, Func<bool> canExecute) : base(param => execute(), param => canExecute())
            => ArgumentNullException.ThrowIfNull(execute);
    }
}