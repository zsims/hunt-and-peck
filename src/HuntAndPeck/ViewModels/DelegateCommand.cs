using System;
using System.Windows.Input;

namespace HuntAndPeck.ViewModels
{
    /// <summary>
    /// Delegate command based on the prism implementation: https://github.com/PrismLibrary/Prism/blob/master/Source/Prism/Commands/DelegateCommand.cs
    /// </summary>
    internal sealed class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute)
            : this(execute, p => true)
        {
        }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public DelegateCommand(Action execute)
            : this(p => execute(), p => true)
        {
        }

        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = p => execute();
            _canExecute = p => canExecute();
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
