using System.Linq;
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
            UpdateGraph(_graph, _network);
            DisplayGraph(_graph, _graphViewer);
        }

        private Graph CreateGraph(string name)
        {
            return new Graph(name)
            {
                Attr = {LayerDirection = LayerDirection.BT},

                LayoutAlgorithmSettings =
                {
                    EdgeRoutingSettings =
                    {
                        UseObstacleRectangles = true,
                        RouteMultiEdgesAsBundles = true,
                        EdgeRoutingMode = EdgeRoutingMode.RectilinearToCenter
                    }
                }
            };
        }

        private void DisplayGraph(Graph graph, GraphViewer graphViewer)
        {
            graphViewer.Graph = graph;
        }

        #endregion

        private void UpdateGraph(Graph graph, StateNetwork network)
        {
            //Remove redundant nodes and edges
            foreach (var graphNode in graph.Nodes.ToList())
            {
                var state = network.GetState(graphNode.LabelText);

                
                if (state == null)
                    graph.RemoveNode(graphNode);
                else
                {
                    foreach (var outEdge in graphNode.OutEdges
                        .ToList()
                        .Where(outEdge => network.GetConnection(state.Name, outEdge.LabelText, outEdge.Target) == null))
                    {
                        graph.RemoveEdge(outEdge);
                    }
                }
            }

            //Add New states and connections
            foreach (var state in network.GetOrderedStates())
            {
                var node = graph.FindNode(state.Name) ?? graph.AddNode(state.Name);

                foreach (var networkConnection in network.GetConnections(state))
                {
                    if (node.OutEdges.Count(edge =>
                        edge.LabelText == networkConnection.Input.Name &&
                        edge.Target == networkConnection.To.Name) == 0)
                    {
                        graph.AddEdge(networkConnection.From.Name, networkConnection.Input.Name, networkConnection.To.Name);
                    }
                }
            }
        }

        public void Update()
        {
            UpdateGraph(_graph, _network);

            _graphViewer.NeedToCalculateLayout = true;
            _graphViewer.Graph = _graphViewer.Graph;
            _graphViewer.NeedToCalculateLayout = false;
            _graphViewer.Graph = _graphViewer.Graph;

            _graph.GeometryGraph.UpdateBoundingBox();
            _graphViewer.Invalidate();
        }
    }
}