using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Persistence.JSon;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;

namespace NetworkCreationTool
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StateNetwork _network;
        private TreeViewItem _rootItem;

        private GraphViewer graphViewer;

        private IViewerNode selectedItem;

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

            foreach (var connection in _network.GetState(parentState).GetConnections()
                .GroupBy(connection => connection.To))
            {
                var child = new TreeViewItem
                {
                    Tag = connection.Key,
                    Header = $"{string.Join(", ", connection.Select(c => c.Input.Name))} : {connection.Key.Name}"
                };
                child.Expanded += ItemExpanded;
                child.Items.Add(null);
                parentTreeViewItem.Items.Add(child);
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            graphViewer = new GraphViewer();
            graphViewer.BindToPanel(GraphViewerPanel);
            GraphViewerPanel.ClipToBounds = true;
            graphViewer.MouseDown += GraphViewer_MouseDown;

            var Graph = new Graph("Test");

            _network = new NetworkJsonSerializer("./test.json").Read();

            foreach (var state in _network.GetOrderedStates())
            {
                var node = Graph.AddNode(state.Name);

                foreach (var networkConnection in _network.GetConnections(state))
                {
                    Graph.AddEdge(networkConnection.From.Name, networkConnection.Input.Name, networkConnection.To.Name);
                }
            }

            Graph.Attr.LayerDirection = LayerDirection.BT;
            Graph.LayoutAlgorithmSettings.EdgeRoutingSettings.UseObstacleRectangles = true;
            Graph.LayoutAlgorithmSettings.EdgeRoutingSettings.RouteMultiEdgesAsBundles = true;
            Graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.RectilinearToCenter;
            graphViewer.Graph = Graph;
        }

        private void GraphViewer_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            outputSelectedItem();
        }

        private void outputSelectedItem()
        {
            var node = graphViewer.ObjectUnderMouseCursor as IViewerNode;

            if (selectedItem == node)
            {
                return;
            }

            SetColor(Color.Black);

            selectedItem = node;

            SetColor(Color.Green);
        }

        private void SetColor(Color color)
        {
            if (selectedItem == null)
            {
                return;
            }

            var drawingNode = (Node) selectedItem.DrawingObject;

            drawingNode.Attr.Color = color;

            graphViewer.Invalidate(selectedItem);
        }
    }
}