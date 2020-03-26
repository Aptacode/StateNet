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
        private readonly IStateNetwork _network;

        #region Events

        public EventHandler<StateUpdatedEventArgs> OnStateUpdated;

        #endregion

        public StateEditorViewModel(IStateNetwork network)
        {
            _network = network;
            Connections = new ObservableCollection<ConnectionViewModel>();
            Inputs = new ObservableCollection<Input>(_network.GetInputs());
            States = new ObservableCollection<State>(_network.GetOrderedStates());
        }

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
            if (State == null)
            {
                return;
            }

            StateName = State.Name;

            Connections.AddRange(
                State
                    .GetConnections()
                    .ToList()
                    .Select(connection => new ConnectionViewModel(_network, connection)));

            foreach (var connectionViewModel in Connections)
            {
                connectionViewModel.OnStateUpdated += (s, e) => OnStateUpdated?.Invoke(this, e);
            }

            Inputs.AddRange(_network.GetInputs());
            States.AddRange(_network.GetOrderedStates());
        }

        #region Properties

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

        public DelegateCommand AddButtonCommand =>
            _addButtonCommand ?? (_addButtonCommand = new DelegateCommand(() =>
            {
                if (NewConnectionSelectedInput == null || NewConnectionSelectedTarget == null)
                {
                    return;
                }

                NewConnectionSelectedExpression = string.IsNullOrEmpty(NewConnectionSelectedExpression) ? "1" : NewConnectionSelectedExpression;

                _network.Connect(State, NewConnectionSelectedInput, NewConnectionSelectedTarget,
                    new ConnectionWeight(NewConnectionSelectedExpression));

                Update();
                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(State));
            }));

        private DelegateCommand _removeButtonCommand;

        public DelegateCommand RemoveButtonCommand =>
            _removeButtonCommand ?? (_removeButtonCommand = new DelegateCommand(() =>
            {
                if (SelectedConnection == null)
                {
                    return;
                }

                _network.Disconnect(SelectedConnection.Source, SelectedConnection.Input, SelectedConnection.Target);
                Update();
                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(State));
            }));

        #endregion
    }
}