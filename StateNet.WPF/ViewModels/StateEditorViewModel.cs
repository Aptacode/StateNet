using System;
using System.Collections.ObjectModel;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateEditorViewModel : BindableBase
    {
        public StateEditorViewModel()
        {
            Connections = new ObservableCollection<ConnectionViewModel>();
            Inputs = new ObservableCollection<Input>();
            States = new ObservableCollection<State>();
        }

        #region Events

        public EventHandler<StateUpdatedEventArgs> OnStateUpdated { get; set; }

        #endregion

        public void Clear()
        {
            StateName = string.Empty;
            Connections.Clear();
            States.Clear();
            Inputs.Clear();
            NewConnectionSelectedInput = null;
            NewConnectionSelectedTarget = null;
            NewConnectionSelectedExpression = null;
        }

        public void Update()
        {
            Clear();

            if (State == null || _network == null)
            {
                return;
            }

            StateName = State.Name;

            Connections.AddRange(
                State
                    .GetOutputConnections()
                    .ToList()
                    .Select(connection => new ConnectionViewModel(_network, connection)));

            foreach (var connectionViewModel in Connections)
            {
                connectionViewModel.OnStateUpdated += (s, e) =>
                {
                    Update();
                    OnStateUpdated?.Invoke(this, e);
                };
            }

            Inputs.AddRange(_network.GetInputs());
            States.AddRange(_network.GetOrderedStates());
        }

        #region Properties

        public IStateNetwork _network;

        public IStateNetwork Network
        {
            get => _network;
            set
            {
                SetProperty(ref _network, value);
                Update();
            }
        }

        private State _state;

        public State State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);

                Update();
            }
        }

        private string _stateName;

        public string StateName
        {
            get => _stateName;
            set
            {
                SetProperty(ref _stateName, value);

                if (State == null || string.IsNullOrEmpty(_stateName) || State.Name == _stateName)
                {
                    return;
                }

                _network.UpdateStateName(State, StateName);

                Update();

                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(State));
            }
        }

        private ObservableCollection<ConnectionViewModel> _connections;

        public ObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => SetProperty(ref _connections, value);
        }

        private ObservableCollection<State> _states;

        public ObservableCollection<State> States
        {
            get => _states;
            set => SetProperty(ref _states, value);
        }

        private ObservableCollection<Input> _inputs;

        public ObservableCollection<Input> Inputs
        {
            get => _inputs;
            set => SetProperty(ref _inputs, value);
        }

        private ConnectionViewModel _selectedConnection;

        public ConnectionViewModel SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(ref _selectedConnection, value);
        }

        private Input _newConnectionSelectedInput;

        public Input NewConnectionSelectedInput
        {
            get => _newConnectionSelectedInput;
            set => SetProperty(ref _newConnectionSelectedInput, value);
        }

        private State _newConnectionSelectedTarget;

        public State NewConnectionSelectedTarget
        {
            get => _newConnectionSelectedTarget;
            set => SetProperty(ref _newConnectionSelectedTarget, value);
        }

        private string _newConnectionSelectedExpression;

        public string NewConnectionSelectedExpression
        {
            get => _newConnectionSelectedExpression;
            set => SetProperty(ref _newConnectionSelectedExpression, value);
        }

        #endregion

        #region Commands

        private DelegateCommand _addButtonCommand;

        public DelegateCommand AddNewConnectionButtonCommand =>
            _addButtonCommand ?? (_addButtonCommand = new DelegateCommand(() =>
            {
                if (NewConnectionSelectedInput == null || NewConnectionSelectedTarget == null)
                {
                    return;
                }

                NewConnectionSelectedExpression = string.IsNullOrEmpty(NewConnectionSelectedExpression)
                    ? "1"
                    : NewConnectionSelectedExpression;

                _network.Connect(State, NewConnectionSelectedInput, NewConnectionSelectedTarget,
                    new ConnectionWeight(NewConnectionSelectedExpression));

                Update();
                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(State));
            }));

        private DelegateCommand _newStateCommand;

        public DelegateCommand NewStateCommand =>
            _newStateCommand ?? (_newStateCommand = new DelegateCommand(() =>
            {
                var newStateName = "New State";
                var newStateCount = 1;
                while (_network.GetState(newStateName + newStateCount) != null)
                {
                    newStateCount++;
                }

                State = _network.CreateState(newStateName + newStateCount);
                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(State));
            }));

        private DelegateCommand _deleteStateCommand;

        public DelegateCommand DeleteStateCommand =>
            _deleteStateCommand ?? (_deleteStateCommand = new DelegateCommand(() =>
            {
                if (State == null)
                {
                    return;
                }

                _network.RemoveState(State);
                State = null;
                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(null));
            }));

        #endregion
    }
}