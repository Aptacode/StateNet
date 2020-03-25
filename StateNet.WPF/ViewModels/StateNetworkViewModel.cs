using System.Windows.Controls;
using Aptacode.StateNet.Network;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateNetworkViewModel : BindableBase
    {
        private void GraphViewer_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            outputSelectedItem();
        }

        private void outputSelectedItem()
        {
            var node = _graphViewer.ObjectUnderMouseCursor as IViewerNode;

            if (_selectedNode == node)
            {
                return;
            }

            SetColor(Color.Black);

            _selectedNode = node;

            SetColor(Color.Green);
        }

        private void SetColor(Color color)
        {
            if (_selectedNode == null)
            {
                return;
            }

            var drawingNode = (Node) _selectedNode.DrawingObject;

            drawingNode.Attr.Color = color;

            _graphViewer.Invalidate(_selectedNode);
        }

        #region Properties

        private GraphViewer _graphViewer;
        private Graph _graph;
        private IViewerNode _selectedNode;

        private DockPanel _graphViewerPanel;

        public DockPanel GraphViewerPanel
        {
            get => _graphViewerPanel;
            set => SetProperty(ref _graphViewerPanel, value);
        }

        private StateNetwork _network;

        public StateNetwork Network
        {
            get => _network;
            set
            {
                SetProperty(ref _network, value);
                LoadNetwork();
            }
        }

        #endregion

        #region Setup

        public StateNetworkViewModel()
        {
            SetupGraphViewerPanel();
            SetupGraphViewer();
        }

        private void SetupGraphViewerPanel()
        {
            GraphViewerPanel = new DockPanel {ClipToBounds = true};
        }

        private void SetupGraphViewer()
        {
            _graphViewer = new GraphViewer();
            _graphViewer.BindToPanel(GraphViewerPanel);
            _graphViewer.MouseDown += GraphViewer_MouseDown;
        }

        #endregion

        #region Load

        private void LoadNetwork()
        {
            _graph = CreateGraph("New Graph");
            LoadGraph(_graph, _network);
            DisplayGraph(_graph, _graphViewer);
        }

        private Graph CreateGraph(string name)
        {
            var graph = new Graph(name);

            graph.Attr.LayerDirection = LayerDirection.BT;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.UseObstacleRectangles = true;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.RouteMultiEdgesAsBundles = true;
            graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.RectilinearToCenter;

            return graph;
        }

        private void LoadGraph(Graph graph, StateNetwork network)
        {
            foreach (var state in network.GetOrderedStates())
            {
                var node = graph.AddNode(state.Name);

                foreach (var networkConnection in network.GetConnections(state))
                {
                    graph.AddEdge(networkConnection.From.Name, networkConnection.Input.Name, networkConnection.To.Name);
                }
            }
        }

        private void DisplayGraph(Graph graph, GraphViewer graphViewer)
        {
            graphViewer.Graph = graph;
        }

        #endregion
    }
}