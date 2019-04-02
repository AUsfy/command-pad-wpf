using System;
using System.Windows.Input;

namespace CmdProject.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _command;
        private readonly Predicate<object> _canExecuteCommand;

        public RelayCommand(Action<object> command, Predicate<object> canExecuteCommand)
        {
            _command = command;
            _canExecuteCommand = canExecuteCommand;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteCommand?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _command(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
