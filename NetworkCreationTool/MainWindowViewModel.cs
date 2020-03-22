using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Aptacode.StateNet;
using Aptacode.StateNet.Connections;
using Aptacode.StateNet.Persistence.JSon;
using Prism.Commands;
using Prism.Mvvm;

namespace NetworkCreationTool
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly NetworkJsonSerializer _networkSerializer;

        private DelegateCommand _addNewInputCommand;

        private DelegateCommand _addNewStateCommand;

        private ObservableCollection<Input> _allInputs;

        private ObservableCollection<State> _allStates;

        private ObservableCollection<State> _connectedStates;
        private DelegateCommand _connectStateCommand;

        private ObservableCollection<State> _disconnectedStates;
        private DelegateCommand _disconnectStateCommand;
        private Network _network;

        private string _newInputName;

        private string _newStateName;

        private DelegateCommand _removeInputCommand;

        private DelegateCommand _removeStateCommand;

        private State _selectedConnectedState;
        private string _selectedConnectedStateExpression;

        private State _selectedDisconnectedState;

        private Input _selectedInput;


        private State _selectedState;

        private DelegateCommand _updateSelectedConnectedStateExpressionCommand;

        public MainWindowViewModel()
        {
            _networkSerializer = new NetworkJsonSerializer("./test.json");
            AllStates = new ObservableCollection<State>();
            AllInputs = new ObservableCollection<Input>();
            ConnectedStates = new ObservableCollection<State>();
            DisconnectedStates = new ObservableCollection<State>();

            Load();

            Application.Current.Exit += Current_Exit;
        }

        public ObservableCollection<State> AllStates
        {
            get => _allStates;
            set => SetProperty(ref _allStates, value);
        }

        public ObservableCollection<Input> AllInputs
        {
            get => _allInputs;
            set => SetProperty(ref _allInputs, value);
        }

        public ObservableCollection<State> ConnectedStates
        {
            get => _connectedStates;
            set => SetProperty(ref _connectedStates, value);
        }

        public ObservableCollection<State> DisconnectedStates
        {
            get => _disconnectedStates;
            set => SetProperty(ref _disconnectedStates, value);
        }

        public State SelectedState
        {
            get => _selectedState;
            set
            {
                SetProperty(ref _selectedState, value);
                LoadConnections();
            }
        }

        public State SelectedConnectedState
        {
            get => _selectedConnectedState;
            set
            {
                SetProperty(ref _selectedConnectedState, value);

                if (_selectedConnectedState == null)
                {
                    SelectedConnectedStateExpression = string.Empty;
                }
                else
                {
                    SelectedConnectedStateExpression = _network[SelectedState, SelectedInput, SelectedConnectedState]
                        .Weight.Expression;
                }
            }
        }

        public State SelectedDisconnectedState
        {
            get => _selectedDisconnectedState;
            set => SetProperty(ref _selectedDisconnectedState, value);
        }

        public Input SelectedInput
        {
            get => _selectedInput;
            set
            {
                SetProperty(ref _selectedInput, value);
                LoadConnections();
            }
        }

        public string NewStateName
        {
            get => _newStateName;
            set => SetProperty(ref _newStateName, value);
        }

        public string NewInputName
        {
            get => _newInputName;
            set => SetProperty(ref _newInputName, value);
        }

        public string SelectedConnectedStateExpression
        {
            get => _selectedConnectedStateExpression;
            set => SetProperty(ref _selectedConnectedStateExpression, value);
        }

        public DelegateCommand RemoveStateCommand =>
            _removeStateCommand ?? (_removeStateCommand = new DelegateCommand(() =>
            {
                if (SelectedState != null)
                {
                    _network.Remove(SelectedState);
                    Refresh();
                }
            }));

        public DelegateCommand AddNewStateCommand =>
            _addNewStateCommand ?? (_addNewStateCommand = new DelegateCommand(() =>
            {
                if (SelectedState == null)
                {
                    return;
                }

                _network.CreateState(NewStateName);
                Refresh();
            }));

        public DelegateCommand AddNewInputCommand =>
            _addNewInputCommand ?? (_addNewInputCommand = new DelegateCommand(() =>
            {
                _network.CreateInput(NewInputName);
                NewInputName = string.Empty;

                Refresh();
            }));

        public DelegateCommand RemoveInputCommand =>
            _removeInputCommand ?? (_removeInputCommand = new DelegateCommand(() =>
            {
                if (SelectedInput == null)
                {
                    return;
                }

                _network.Remove(SelectedInput);
                Refresh();
            }));

        public DelegateCommand UpdateSelectedConnectedStateExpressionCommand =>
            _updateSelectedConnectedStateExpressionCommand ?? (_updateSelectedConnectedStateExpressionCommand =
                new DelegateCommand(() =>
                {
                    _network.Connect(SelectedState, SelectedInput, SelectedConnectedState,
                        new ConnectionWeight(SelectedConnectedStateExpression));
                }));

        public DelegateCommand ConnectStateCommand =>
            _connectStateCommand ?? (_connectStateCommand = new DelegateCommand(() =>
            {
                if (SelectedDisconnectedState == null)
                {
                    return;
                }

                _network.Connect(SelectedState, SelectedInput, SelectedDisconnectedState,
                    new ConnectionWeight(1.ToString()));

                LoadConnections();
            }));

        public DelegateCommand DisconnectStateCommand =>
            _disconnectStateCommand ?? (_disconnectStateCommand = new DelegateCommand(() =>
            {
                if (SelectedConnectedState == null)
                {
                    return;
                }

                _network.Clear(SelectedState, SelectedInput, SelectedConnectedState);

                LoadConnections();
            }));

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Save();
        }

        public void Load()
        {
            _network = _networkSerializer.Read();
            Refresh();
        }

        public void Save()
        {
            _networkSerializer.Write(_network);
        }

        public void Refresh()
        {
            AllStates.Clear();
            AllInputs.Clear();
            ConnectedStates.Clear();
            DisconnectedStates.Clear();

            AllStates.AddRange(_network.GetStates());
            AllInputs.AddRange(_network.GetInputs());

            NewStateName = string.Empty;
            NewInputName = string.Empty;
        }

        public void LoadConnections()
        {
            ConnectedStates.Clear();
            DisconnectedStates.Clear();

            if (SelectedState == null || SelectedInput == null)
            {
                return;
            }

            var connections = _network[SelectedState, SelectedInput];
            ConnectedStates.AddRange(connections.Select(connection => connection.To).Distinct());
            DisconnectedStates.AddRange(AllStates.Where(state => !ConnectedStates.Contains(state)));
        }
    }
}