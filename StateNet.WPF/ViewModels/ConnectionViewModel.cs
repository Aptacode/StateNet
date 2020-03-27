using System;
using System.Collections.ObjectModel;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class ConnectionViewModel : BindableBase
    {
        private readonly IStateNetwork _network;

        public ConnectionViewModel(IStateNetwork network, Connection connection)
        {
            _network = network;
            Connection = connection;

            Inputs = new ObservableCollection<Input>(_network.GetInputs());
            States = new ObservableCollection<State>(_network.GetOrderedStates());

            Source = Connection.Source;
            Input = Connection.Input;
            Target = Connection.Target;
            Expression = Connection.ConnectionWeight.Expression;
        }

        #region Events

        public EventHandler<StateUpdatedEventArgs> OnStateUpdated { get; set; }

        #endregion

        #region Properties

        private State _source;
        private Input _input;
        private State _target;
        private string _expression;
        private Connection _connection;

        public State Source
        {
            get => _source;
            set
            {
                SetProperty(ref _source, value);
                Connection.Source = _source;
            }
        }

        public Input Input
        {
            get => _input;
            set
            {
                SetProperty(ref _input, value);

                if (_input == null || Connection == null || Connection.Input == _input)
                {
                    return;
                }

                _network.Disconnect(Connection.Source, Connection.Input, Connection.Target);
                Connection.Input = _input;
                _network.Connect(Connection.Source, Connection.Input, Connection.Target, Connection.ConnectionWeight);

                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(Source));
            }
        }

        public State Target
        {
            get => _target;
            set
            {
                SetProperty(ref _target, value);

                if (_target == null || Connection == null || Connection.Target == _target)
                {
                    return;
                }

                _network.Disconnect(Connection.Source, Connection.Input, Connection.Target);
                Connection.Target = _target;
                _network.Connect(Connection.Source, Connection.Input, Connection.Target, Connection.ConnectionWeight);

                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(Source));
            }
        }

        public string Expression
        {
            get => _expression;
            set
            {
                SetProperty(ref _expression, value);

                if (string.IsNullOrEmpty(_expression) || Connection == null ||
                    Connection.ConnectionWeight.Expression == _expression)
                {
                    return;
                }

                _network.Disconnect(Connection.Source, Connection.Input, Connection.Target);
                Connection.ConnectionWeight = new ConnectionWeight(_expression);
                _network.Connect(Connection.Source, Connection.Input, Connection.Target, Connection.ConnectionWeight);

                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(Source));
            }
        }

        public Connection Connection
        {
            get => _connection;
            set => SetProperty(ref _connection, value);
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

        #endregion

        #region Commands

        private DelegateCommand _removeButtonCommand;

        public DelegateCommand RemoveButtonCommand =>
            _removeButtonCommand ?? (_removeButtonCommand = new DelegateCommand(() =>
            {
                _network.Disconnect(Connection.Source, Connection.Input, Connection.Target);

                OnStateUpdated?.Invoke(this, new StateUpdatedEventArgs(Source));
            }));

        #endregion
    }
}