using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CmdProject.ViewModels
{
    public abstract class BaseViewModel:INotifyPropertyChanged
    {
        private static BaseViewModel _viewModel;

        public BaseViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}