using System.Windows;
using CmdProject.ViewModels;

namespace CmdProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainViewWindowModel() {ViewModel = new MainViewModel()};
        }
    }
}
