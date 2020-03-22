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
        private Network _network;
        private TreeViewItem _rootItem;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Load();
        }

        private void Load()
        {
            _network = new NetworkJsonSerializer("./test.json").Read();
            _rootItem = new TreeViewItem();
            _rootItem.Tag = _network.StartState;
            _rootItem.Header = _network.StartState.Name;
            _rootItem.Items.Add(null);
            _rootItem.Expanded += ItemExpanded;
            TreeView.Items.Add(_rootItem);
        }

        private void ItemExpanded(object sender, RoutedEventArgs e)
        {
            if (!(sender is TreeViewItem parentTreeViewItem))
            {
                return;
            }

            var parentState = parentTreeViewItem.Tag as State;

            if (parentTreeViewItem.Items.Count != 1 || parentTreeViewItem.Items[0] != null)
            {
                return;
            }

            parentTreeViewItem.Items.Clear();

            foreach (var connection in _network.GetState(parentState).GetConnections())
            {
                var child = new TreeViewItem
                {
                    Tag = connection.To,
                    Header = $"{connection.Input.Name} : {connection.To.Name}"
                };
                child.Expanded += ItemExpanded;
                child.Items.Add(null);
                parentTreeViewItem.Items.Add(child);
            }
        }
    }
}