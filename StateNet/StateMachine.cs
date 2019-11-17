using Aptacode.StateNet.Exceptions;
using Aptacode.StateNet.Transitions;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Aptacode.StateNet
{
    public class StateMachine : IDisposable
    {
        private readonly ConcurrentQueue<string> inputQueue;
        private bool _isRunning;

        static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        readonly StateTransitionTable _stateTransitionTable;
        readonly StateCollection _states;
        readonly InputCollection _inputs;

        /// <summary>
        /// Governs the transitions between states based on the inputs it receives
        /// </summary>
        public StateMachine(StateCollection states, InputCollection inputs, string initialState)
        {
            _states = states;
            _inputs = inputs;
            _stateTransitionTable = new StateTransitionTable(_states, _inputs);
            inputQueue = new ConcurrentQueue<string>();
            SetInitialState(initialState);
        }

        public void Start()
        {
            new TaskFactory().StartNew(() =>
            {
                _isRunning = true;
                while (_isRunning)
                {
                    NextTransition();
                    Task.Delay(1).ConfigureAwait(false);
                }
            });
        }

        public void Stop()
        {
            _isRunning = false;
        }

        void SetInitialState(string initialState)
        {
            if(_states.Contains(initialState))
            {
                State = initialState;
            } else
            {
                State = _states.First();
            }
        }

        /// <summary>
        /// Returns the current State
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        /// Returns the last input
        /// </summary>
        public string LastInput { get; private set; }

        public event EventHandler<StateTransitionArgs> OnTransition;
        public event EventHandler<InvalidStateTransitionArgs> OnInvalidTransition;

        /// <summary>
        /// Define a new transition
        /// </summary>
        /// <param name="transition"></param>
        public void Define(Transition transition)
        {
            _stateTransitionTable.Set(transition);
        }

        /// <summary>
        /// Set a transition to 'Undefined'
        /// </summary>
        /// <param name="transition"></param>
        public void Clear(Transition transition) => _stateTransitionTable.Clear(transition) ;

        /// <summary>
        /// Apply the transition which relates to the given input on the current state
        /// </summary>
        /// <param name="input"></param>
        public void Apply(string input)
        {
            inputQueue.Enqueue(input);
        }

        private void NextTransition()
        {
            if(inputQueue.TryDequeue(out var input)){
                try
                {
                    var transition = GetValidTransition(State, input);
                    var nextState = transition.Apply();
                    LastInput = input;
                    UpdateState(nextState);
                }
                catch
                {
                    OnInvalidTransition?.Invoke(this, new InvalidStateTransitionArgs(State, input));
                }
            }
        }

        ValidTransition GetValidTransition(string state, string input)
        {
            if(_stateTransitionTable.Get(state, input) is ValidTransition validTransition)
            {
                return validTransition;
            }

            throw new InvalidTransitionException(State, input);
        }

        void UpdateState(string nextState)
        {
            var oldState = State;
            State = nextState;
            OnTransition?.Invoke(this, new StateTransitionArgs(oldState, LastInput, nextState));
        }

        public void Dispose()
        {
            Stop();
        }
    }
}