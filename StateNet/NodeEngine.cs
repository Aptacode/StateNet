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
        private readonly List<Node> _history;
        private readonly NodeChooser _nodeChooser;
        private readonly ConcurrentQueue<string> _inputQueue;
        private readonly Dictionary<Node, List<Action>> _callbackDictionary;
        private bool _isRunning;
        public Node CurrentNode { get; private set; }

        public NodeEngine(INodeGraph nodeGraph)
        {
            _nodeGraph = nodeGraph;
            _history = new List<Node>();
            _nodeChooser = new NodeChooser(_history);
            _callbackDictionary = new Dictionary<Node, List<Action>>();
            _inputQueue = new ConcurrentQueue<string>();
            _isRunning = false;
        }

        public event EngineEvent OnFinished;

        public event EngineEvent OnStarted;

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
                    _history.Add(sender);

                    NotifySubscribers(sender);
                };
            }
        }

        private void NotifySubscribers(Node node)
        {
            if (_callbackDictionary.ContainsKey(node))
            {
                _callbackDictionary[node]?.ForEach(callback =>
                {
                    new TaskFactory().StartNew(() =>
                    {
                        callback?.Invoke();
                    }).ConfigureAwait(false);
                });
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

        public List<Node> GetHistory() => _history;

        public void Start()
        {
            if (_nodeGraph.IsValid())
            {
                SubscribeToNodesVisited();
                SubscribeToEndNodes();
                OnStarted?.Invoke(this);
                CurrentNode = _nodeGraph.StartNode;
                _history.Add(CurrentNode);

                new TaskFactory().StartNew(async () =>
                {
                    _isRunning = true;

                    while (_isRunning)
                    {
                        NextTransition();
                        await Task.Delay(1).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }
            else
            {
                throw new Exception();
            }
        }
        public void Stop() => _isRunning = false;
        public void Apply(string actionName) => _inputQueue.Enqueue(actionName);

        private void NextTransition()
        {
            if (_inputQueue.TryDequeue(out var actionName))
            {
                CurrentNode.UpdateChoosers();
                var nextNode = Next(CurrentNode, actionName);
                if (nextNode != null)
                {
                    CurrentNode.Exit();
                    CurrentNode = nextNode;
                    CurrentNode.Visit();
                }
            }
        }

        public Node Next(Node node, string actionName) => _nodeChooser.Next(_nodeGraph[node, actionName]);
    }
}
