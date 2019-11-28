using Aptacode.StateNet.NodeMachine.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aptacode.StateNet.NodeMachine
{
    public delegate void EngineEvent(Engine sender);

    public class Engine
    {
        private List<Node> _visitLog;

        public Engine(Node startNode)
        {
            StartNode = startNode;
            _visitLog = new List<Node>();
        }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        private void SubscribeToEndNodes()
        {
            foreach(var node in Flatten(StartNode, new HashSet<Node>()))
            {
                node.OnVisited += (s) =>
                {
                    _visitLog.Add(s);
                };

                if(node is EndNode)
                {
                    node.OnVisited += (s) =>
                    {
                        OnFinished?.Invoke(this);
                    };
                }
            }
        }

        public HashSet<Node> Flatten(Node node, HashSet<Node> visitedNodes)
        {
            if(!visitedNodes.Contains(node))
            {
                visitedNodes.Add(node);
                foreach(var exitNode in node.GetNextNodes())
                {
                    Flatten(exitNode, visitedNodes);
                }
            }

            return visitedNodes;
        }

        public IEnumerable<Node> GetVisitLog() => _visitLog;

        public bool IsValid()
        {
            foreach(var node in Flatten(StartNode, new HashSet<Node>()))
            {
                if(node is EndNode)
                {
                    return true;
                }
            }

            return false;
        }

        public void Start()
        {
            if(IsValid())
            {
                SubscribeToEndNodes();
                OnStarted?.Invoke(this);
                StartNode.Visit();
            } else
            {
                throw new Exception();
            }
        }

        public Node EndNode { get; }

        public Node StartNode { get; }
    }
}
