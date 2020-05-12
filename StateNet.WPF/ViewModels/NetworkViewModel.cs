using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class NetworkViewModel : BindableBase
    {
        public NetworkViewModel()
        {
            SetupGraphViewerPanel();
        }

        #region Events

        public EventHandler<StateViewModel> OnStateSelected { get; set; }

        #endregion

        #region Methods

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

        public void Load()
        {
            SetupGraphViewer();
            _graphViewer.Graph = CreateGraph("TestGraph");
            _graphViewer.Invalidate();
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
            foreach (var state in StateNetwork.Model.GetOrderedStates())
            {
                var node = newGraph.AddNode(state.Name);

                foreach (var networkConnection in StateNetwork.Model.GetConnections(state))
                {
                    if (networkConnection.Source == null || networkConnection.Input == null ||
                        networkConnection.Target == null)
                    {
                        continue;
                    }

                    if (node.OutEdges.Count(edge =>
                        edge.LabelText == networkConnection.Input.Name &&
                        edge.Target == networkConnection.Target.Name) == 0)
                    {
                        newGraph.AddEdge(networkConnection.Source.Name, networkConnection.Input.Name,
                            networkConnection.Target.Name);
                    }
                }
            }

            if (StateNetwork.Model.StartState != null)
            {
                var drawingNode = newGraph.FindNode(StateNetwork.Model.StartState.Name);
                drawingNode.Attr.Color = Color.Green;
            }

            return newGraph;
        }

        #endregion

        #region Properties

        private Color selectedNodeColor = Color.Black;

        private StateNetworkViewModel _stateNetwork;

        public StateNetworkViewModel StateNetwork
        {
            get => _stateNetwork;
            set
            {
                SetProperty(ref _stateNetwork, value);
                Load();
            }
        }

        private GraphViewer _graphViewer;
        private IViewerNode _selectedNode;

        private DockPanel _graphViewerPanel;

        public DockPanel GraphViewerPanel
        {
            get => _graphViewerPanel;
            set => SetProperty(ref _graphViewerPanel, value);
        }

        #endregion


        #region Code Behind

        private void GraphViewer_MouseDown(object sender, MsaglMouseEventArgs e)
        {
            outputSelectedItem();
        }

        private void outputSelectedItem()
        {
            var node = _graphViewer.ObjectUnderMouseCursor as IViewerNode;
            var selectedState = StateNetwork.Model.GetState(node?.Node?.LabelText);
            var previousState = StateNetwork.Model.GetState(_selectedNode?.Node?.LabelText);
            var selectedColor = previousState == StateNetwork.Model.StartState ? Color.Green : Color.Black;
            SetColor(selectedColor);

            if (_selectedNode == node || selectedState == null)
            {
                return;
            }

            OnStateSelected?.Invoke(this, StateNetwork.States.FirstOrDefault(s => s.Name == node?.Node?.LabelText));

            selectedColor = selectedState.Equals(StateNetwork.Model.StartState) ? Color.Green : Color.Blue;

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

        #endregion

        #region Commands

        private DelegateCommand _refreshCommand;

        public DelegateCommand RefreshCommand =>
            _refreshCommand ?? (_refreshCommand = new DelegateCommand(Update));

        #endregion
    }
}