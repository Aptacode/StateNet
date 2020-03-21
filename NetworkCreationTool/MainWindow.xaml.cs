using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Aptacode.StateNet;
using Aptacode.StateNet.Persistence.JSon;

namespace NetworkCreationTool
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Network network;
        private TreeViewItem top;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();


            var _networkSerializer = new NetworkJsonSerializer("./test.json");
            network = _networkSerializer.Read();

            top = new TreeViewItem();
            top.Tag = network.StartState;
            top.Header = network.StartState.Name;
            top.Items.Add(null);
            top.Expanded += ItemExpanded;
            TreeView.Items.Add(top);
        }

        private void ItemExpanded(object sender, RoutedEventArgs e)
        {
            var parentTreeViewItem = sender as TreeViewItem;
            var parentState = parentTreeViewItem.Tag as State;

            if (parentTreeViewItem.Items.Count != 1 || parentTreeViewItem.Items[0] != null)
                return;

            parentTreeViewItem.Items.Clear();

            foreach (var connection in network.GetState(parentState).GetConnections())
            {
                TreeViewItem child = new TreeViewItem();
                child.Tag = connection.To;
                child.Header = $"{connection.Input.Name} : {connection.To.Name}";
                child.Expanded += ItemExpanded;
                child.Items.Add(null);
                parentTreeViewItem.Items.Add(child);
            }
        }
    }
}