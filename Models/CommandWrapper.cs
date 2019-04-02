using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Models
{
    public class CommandWrapper : INotifyPropertyChanged
    {
        private CommandStatus _status;

        public CommandModel CommandModel { get; set; }
        public CommandStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");

            }
        }
        public CancellationTokenSource cancellationTokenSource { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CommandWrapper()
        {
            CommandModel = new CommandModel();
            Status = CommandStatus.Inactive;
        }
        public CommandWrapper(CommandModel commandModel, CommandStatus status = CommandStatus.Inactive)
        {
            CommandModel = commandModel;
            Status = status;

        }
    }

    public enum CommandStatus
    {
        Inactive,
        Running
    }
}
