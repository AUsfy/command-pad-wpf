using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Models;
using System;
using Core;
using System.Threading;

namespace CmdProject.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
    }

    public class MainViewWindowModel : BaseViewModel
    {
        public MainViewWindowModel() : base()
        {
        }
        private ICommandRunner _commandRunner;
        private ICommandRunner CommandRunner { get { return _commandRunner ?? (_commandRunner = new CommandRunner()); } }

        private readonly IDictionary<Guid, bool> buttonPlayingStatus = new Dictionary<Guid, bool>();
        private RelayCommand _playCommand;

        public ICommand DisplayCommandView { get { return new RelayCommand(action => ViewModel = new CommandViewModel(UpdateViewModel, AddCommandToList), null); } }
        public ICommand DisplayListCommandView { get { return new RelayCommand(action => ViewModel = new MainViewModel(), null); } }

        public ICommand RunSelectedCommand { get { return new RelayCommand(action => ViewModel = new CommandViewModel(UpdateViewModel, AddCommandToList), null); } }
        public ICommand Edit { get { return new RelayCommand(action => ViewModel = new CommandViewModel(UpdateViewModel, AddCommandToList, SelectedCommand), null); } }
        public CommandWrapper SelectedCommand { get; set; }
        public ICommand SelectedCommandCmd { get { return new RelayCommand(action  => ViewModel = new CommandViewModel(UpdateViewModel, AddCommandToList, SelectedCommand), null); } }
        private static ObservableCollection<CommandWrapper> _commandList = new ObservableCollection<CommandWrapper>(DbWrapper.Commands);


        public ICommand PlayCommand
        {
            get
            {
                return  new RelayCommand(async (command) =>
                {
                    var cmd = command as CommandWrapper;
                    if (cmd != null) { 
                        CommandStatus commandStatus = cmd.Status;
                        if (commandStatus == CommandStatus.Inactive)
                        {
                            cmd.Status = CommandStatus.Running;
                            cmd.cancellationTokenSource = new CancellationTokenSource();

                            var result = await CommandRunner.RunCommandAsync(cmd.CommandModel.Command, cmd.CommandModel.RunAsAdmin,cmd.cancellationTokenSource.Token);
                        }
                        else if (commandStatus == CommandStatus.Running)
                        {
                            cmd.Status = CommandStatus.Inactive;
                            cmd.cancellationTokenSource?.Cancel(); 
                        }
                    }
                }, null);
            }
        }
        public ObservableCollection<CommandWrapper> CommandList
        {
            get
            {
                return _commandList;
            }
        }
        
        private void UpdateViewModel(BaseViewModel vM)
        {
            ViewModel = vM;
        }

        private void AddCommandToList(CommandModel cmdModel)
        {
            
            if (cmdModel == null)
                return;
            
            foreach(var cmd in CommandList)
            {
                var command = cmd.CommandModel;
                if(command.Id == cmdModel.Id)
                {
                    command.Command = cmdModel.Command;
                    command.CommandName = cmdModel.CommandName;
                    return;
                }
            }
            CommandList.Add(new CommandWrapper(cmdModel, CommandStatus.Inactive));
        }
    }
}

