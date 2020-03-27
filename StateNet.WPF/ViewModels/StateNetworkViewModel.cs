using System;
using System.Linq;
using System.Windows.Controls;
using Aptacode.StateNet.Interfaces;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateNetworkViewModel : BindableBase
    {
        #region Events

        public EventHandler<SelectedStateEventArgs> OnStateSelected { get; set; }

        #endregion

        private Color selectedNodeColor = Color.Black;

        private void GraphViewer_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            outputSelectedItem();
        }

        private void outputSelectedItem()
        {
            var node = _graphViewer.ObjectUnderMouseCursor as IViewerNode;
            var selectedState = _network.GetState(node?.Node?.LabelText);
            var previousState = _network.GetState(_selectedNode?.Node?.LabelText);
            var selectedColor = (previousState == _network.StartState) ? Color.Green : Color.Black;
            SetColor(selectedColor);

            if (_selectedNode == node || selectedState == null)
            {
                return;
            }

            OnStateSelected?.Invoke(this, new SelectedStateEventArgs(selectedState));

            selectedColor = selectedState.Equals(_network.StartState) ? Color.Green : Color.Blue;

            _selectedNode = node;

            SetColor(selectedColor);
            selectedNodeColor = selectedColor;
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

        public void Update()
        {
            _graphViewer.Graph = CreateGraph("TestGraph");
            _graphViewer.Invalidate();
        }

        #region Properties

        private GraphViewer _graphViewer;
        private IViewerNode _selectedNode;

        private DockPanel _graphViewerPanel;

        public DockPanel GraphViewerPanel
        {
            get => _graphViewerPanel;
            set => SetProperty(ref _graphViewerPanel, value);
        }

        private IStateNetwork _network;

        public IStateNetwork Network
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
            _graphViewer.Graph = CreateGraph("TestGraph");
        }

        private Graph CreateGraph(string name)
        {
            _selectedNode = null;

            var newGraph = new Graph(name)
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
            //Add New states and connections
            foreach (var state in _network.GetOrderedStates())
            {
                var node = newGraph.AddNode(state.Name);

                foreach (var networkConnection in _network.GetConnections(state))
                {
                    if (node.OutEdges.Count(edge =>
                        edge.LabelText == networkConnection.Input.Name &&
                        edge.Target == networkConnection.Target.Name) == 0)
                    {
                        newGraph.AddEdge(networkConnection.Source.Name, networkConnection.Input.Name,
                            networkConnection.Target.Name);
                    }
                }
            }

            if (_network.StartState != null)
            {
                var drawingNode = (Node)newGraph.FindNode(_network.StartState.Name);
                drawingNode.Attr.Color = Color.Green;
            }

            return newGraph;
        }

        #endregion
    }
}