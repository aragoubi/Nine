using System;
using System.Windows.Input;

namespace Nine.MVVM
{
    /// <summary>
    /// ICommand implementation with a specified parameter
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
    ///             _command = new ParametrizedRelayCommand(
    ///                 parameter => { /* ... */ }
    ///             );
    ///         }
    ///         return _command;
    ///     }
    /// }
    /// </code>
    /// </example>
    public class ParametrizedRelayCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Predicate<object> condition;

        public ParametrizedRelayCommand(Action<object> action)
        {
            this.action = action;
            condition = parameter => true;
        }

        public ParametrizedRelayCommand(Action<object> action, bool condition)
        {
            this.action = action;
            this.condition = parameter => condition;
        }

        public ParametrizedRelayCommand(Action<object> action, Predicate<object> condition)
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
            return condition.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            action.Invoke(parameter);
        }
    }
}