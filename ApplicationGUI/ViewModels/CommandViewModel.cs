using System;
using System.Windows.Input;
using Models;

namespace CmdProject.ViewModels
{
    public class CommandViewModel : BaseViewModel
    {
        private CommandModel _cmd;
        private Action<BaseViewModel> _updateViewModel;
        private Action<CommandModel> _addCommandsToList;


        public CommandViewModel(Action<BaseViewModel> updateViewModel, Action<CommandModel> addCommandsToList, CommandWrapper cmd = null)
        {
            _updateViewModel = updateViewModel;
            _addCommandsToList = addCommandsToList;
            _cmd =  cmd?.CommandModel??new CommandModel();
        }
        
        public string CommandName {
            get { return _cmd.CommandName; }
            set
            {
                if (_cmd.CommandName != value)
                {
                    _cmd.CommandName = value;
                }
            }
        }

        public string Command {
            get { return _cmd.Command; }
            set
            {
                if (_cmd.Command != value)
                {
                    _cmd.Command = value;
                }
            }
        }

        public bool IsModeAdminChecked
        {
            get     { return _cmd.RunAsAdmin; }
            set
            {
                _cmd.RunAsAdmin = value;
                OnPropertyChanged();
            }
        }
        public ICommand Save
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (!string.IsNullOrEmpty(_cmd.CommandName) && !string.IsNullOrEmpty(_cmd.Command))
                    {
                        _addCommandsToList(_cmd);
                        _updateViewModel(new MainViewModel());
                        DbWrapper.InsertOrUpdateCommand(_cmd);
                    }
                }, null);
            }
        }
    }
}
