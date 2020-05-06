using System.Windows;
using Aptacode.StateNet.NetworkCreationTool.ViewModels;

namespace Aptacode.StateNet.NetworkCreationTool.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}