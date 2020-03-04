using System.Windows;
using WPFDemo.ViewModels;

namespace WPFDemo.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StateViewModel stateViewModel;

        public MainWindow()
        {
            InitializeComponent();

            stateViewModel = new StateViewModel();
            stateViewModel.Name = "A1";
            ContentPresenter.Content = stateViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stateViewModel.IsActive = !stateViewModel.IsActive;
        }
    }
}