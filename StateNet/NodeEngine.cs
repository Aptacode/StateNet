using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aptacode.StateNet.Events;
using Aptacode.StateNet.Interfaces;

namespace Aptacode.StateNet
{
    public class NodeEngine : INodeEngine
    {
        private readonly INodeGraph _nodeGraph;
        private readonly List<Node> _visitLog;
        public Node CurrentNode { get; private set; }

        private readonly ConcurrentQueue<string> inputQueue;
        private bool _isRunning;

        public NodeEngine(INodeGraph nodeGraph)
        {
            _nodeGraph = nodeGraph;
            _visitLog = new List<Node>();
            _callbackDictionary = new Dictionary<Node, List<Action>>();
            inputQueue = new ConcurrentQueue<string>();
            _isRunning = false;
        }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

        private readonly Dictionary<Node, List<Action>> _callbackDictionary;

        private void SubscribeToEndNodes()
        {
            foreach (var node in _nodeGraph.GetEndNodes())
            {
                node.OnVisited += (s) =>
                {
                    OnFinished?.Invoke(this);
                };
            }
        }

        private void SubscribeToNodesVisited()
        {
            foreach (var node in _nodeGraph.GetAll())
            {
                node.OnVisited += (sender) =>
                {
                    _visitLog.Add(sender);

                    _callbackDictionary[node]?.ForEach(callback =>
                    {
                        new TaskFactory().StartNew(() =>
                        {
                            callback?.Invoke();
                        });
                    });
                };
            }
        }

        public void Subscribe(Node node, Action callback)
        {
            if (!_callbackDictionary.TryGetValue(node, out var actions))
            {
                actions = new List<Action>();
                _callbackDictionary.Add(node, actions);
            }

            actions.Add(callback);
        }

        public void Unsubscribe(Node node, Action callback)
        {
            if (_callbackDictionary.TryGetValue(node, out var actions))
            {
                actions.Remove(callback);
            }
        }

        public IEnumerable<Node> GetVisitLog() => _visitLog;

        public void Start()
        {
            if (_nodeGraph.IsValid())
            {
                SubscribeToNodesVisited();
                SubscribeToEndNodes();
                OnStarted?.Invoke(this);
                CurrentNode = _nodeGraph.StartNode;
                _visitLog.Add(CurrentNode);

                new TaskFactory().StartNew(async () =>
                {
                    _isRunning = true;

                    while (_isRunning)
                    {
                        NextTransition();
                        await Task.Delay(1).ConfigureAwait(false);
                    }
                });


            }
            else
            {
                throw new Exception();
            }
        }
        public void Stop() => _isRunning = false;
        public void Apply(string actionName) => inputQueue.Enqueue(actionName);

        private void NextTransition()
        {
            if (inputQueue.TryDequeue(out var actionName))
            {
                try
                {
                    CurrentNode.UpdateChoosers();
                    var nextNode = _nodeGraph.Next(CurrentNode, actionName);
                    if (nextNode != null)
                    {
                        CurrentNode.Exit();
                        CurrentNode = nextNode;
                        CurrentNode.Visit();
                    }
                }
                catch
                {

                }
            }
        }
    }
}
