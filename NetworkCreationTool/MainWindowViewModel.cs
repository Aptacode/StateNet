using System.Collections.ObjectModel;
using System.Linq;
using Aptacode.StateNet;
using Aptacode.StateNet.Connections.Weights;
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

        private ObservableCollection<State> _disconnectedStates;
        private Network _network;

        private string _newInputName;

        private string _newStateName;

        private DelegateCommand _removeInputCommand;

        private DelegateCommand _removeStateCommand;

        private State _selectedConnectedState;

        private State _selectedDisconnectedState;

        private Input _selectedInput;


        private State _selectedState;

        public MainWindowViewModel()
        {
            _networkSerializer = new NetworkJsonSerializer("./test.json");
            AllStates = new ObservableCollection<State>();
            AllInputs = new ObservableCollection<Input>();
            ConnectedStates = new ObservableCollection<State>();
            DisconnectedStates = new ObservableCollection<State>();

            Load();
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

                if (SelectedConnectedState == null)
                {
                    return;
                }

                _network.Clear(SelectedState, SelectedInput, SelectedConnectedState);

                LoadConnections();
            }
        }

        public State SelectedDisconnectedState
        {
            get => _selectedDisconnectedState;
            set
            {
                SetProperty(ref _selectedDisconnectedState, value);

                if (SelectedDisconnectedState == null)
                {
                    return;
                }

                _network.Connect(SelectedState, SelectedInput, SelectedDisconnectedState, new StaticWeight(1));

                LoadConnections();
            }
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

        public void Load()
        {
            _network = _networkSerializer.Read();
            Refresh();
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
            ConnectedStates.AddRange(connections.Select(connection => _network[connection.To]).Distinct());
            DisconnectedStates.AddRange(AllStates.Where(state => !ConnectedStates.Contains(state)));
        }
    }
}