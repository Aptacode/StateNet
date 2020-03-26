using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Persistence.Json;
using Aptacode.StateNet.WPF.ViewModels;
using Prism.Mvvm;

namespace Aptacode.StateNet.NetworkCreationTool
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IStateNetwork _network;
        private readonly StateNetworkJsonSerializer _stateNetworkSerializer;
        private StateEditorViewModel _stateEditorViewModel;

        private StateNetworkViewModel _stateNetworkViewModel;

        public MainWindowViewModel()
        {
            _stateNetworkSerializer = new StateNetworkJsonSerializer("./test.json");
            _network = _stateNetworkSerializer.Read();

            StateNetworkViewModel = new StateNetworkViewModel();
            StateNetworkViewModel.OnStateSelected += (s, e) => { StateEditorViewModel.State = e.State; };
            StateEditorViewModel = new StateEditorViewModel(_network);
            StateEditorViewModel.OnStateUpdated += (s, e) => { StateNetworkViewModel.Update(); };
        }

        public StateNetworkViewModel StateNetworkViewModel
        {
            get => _stateNetworkViewModel;
            set => SetProperty(ref _stateNetworkViewModel, value);
        }

        public StateEditorViewModel StateEditorViewModel
        {
            get => _stateEditorViewModel;
            set => SetProperty(ref _stateEditorViewModel, value);
        }


        public void Load()
        {
            StateNetworkViewModel.Network = _network;
        }

        public void Save()
        {
            _stateNetworkSerializer.Write(_network);
        }
    }
}