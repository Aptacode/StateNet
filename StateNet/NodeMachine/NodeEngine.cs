using Aptacode.StateNet.NodeMachine.Events;
using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;

namespace Aptacode.StateNet.NodeMachine
{
    public class NodeEngine
    {
        private readonly NodeGraph _nodeGraph;
        private readonly List<Node> _visitLog;

        public NodeEngine(NodeGraph nodeGraph)
        {
            _nodeGraph = nodeGraph;
            _visitLog = new List<Node>();
        }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        private void SubscribeToEndNodes()
        {
            foreach(var node in _nodeGraph.GetEndNodes())
            {
                if(node is EndNode)
                {
                    node.OnVisited += (s) =>
                    {
                        OnFinished?.Invoke(this);
                    };
                }
            }
        }

        private void SubscribeToNodesVisited()
        {
            foreach(var node in _nodeGraph.GetAll())
            {
                node.OnVisited += (sender) =>
                {
                    _visitLog.Add(sender);
                };
            }
        }

        public IEnumerable<Node> GetVisitLog() => _visitLog;

        public void Start()
        {
            if(_nodeGraph.IsValid())
            {
                SubscribeToNodesVisited();
                SubscribeToEndNodes();
                OnStarted?.Invoke(this);
                _nodeGraph.StartNode.Visit();
            } else
            {
                throw new Exception();
            }
        }
    }
}
