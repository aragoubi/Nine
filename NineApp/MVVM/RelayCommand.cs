using System;
using System.Windows.Input;

namespace Nine.MVVM
{
    /// <summary>
    /// ICommand implementation without parameter
    /// </summary>
    /// <example>
    /// How to use
    /// <code>
    /// private ICommand _command = null;
    /// public ICommand Property
    /// {
    ///     get
    ///     {
    ///         if(_command == null)
    ///         {
    ///             _command = new RelayCommand(
    ///                 () => { /* ... */ }
    ///             );
    ///         }
    ///         return _command;
    ///     }
    /// }
    /// </code>
    /// </example>
    public class RelayCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> condition;

        public RelayCommand(Action action)
        {
            this.action = action;
            condition = () => true;
        }

        public RelayCommand(Action action, bool condition)
        {
            this.action = action;
            this.condition = () => condition;
        }

        public RelayCommand(Action action, Func<bool> condition)
        {
            this.action = action;
            this.condition = condition;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return condition.Invoke();
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}