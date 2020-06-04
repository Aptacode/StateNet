using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.Wpf.ViewModels
{
    public class ConnectionEditorViewModel : BindableBase
    {
        public ConnectionEditorViewModel(StateNetworkViewModel stateNetworkViewModel)
        {
            StateNetwork = stateNetworkViewModel;
            Load();
        }

        #region Events

        public event EventHandler<ConnectionViewModel> OnConnectionModified;

        #endregion

        #region Methods

        public void Clear()
        {
            States.Clear();
            Inputs.Clear();
        }

        public void Load()
        {
            States.AddRange(StateNetwork.States);
            Inputs.AddRange(StateNetwork.Inputs);
        }

        public void Refresh()
        {
            Clear();
            Load();
        }

        #endregion

        #region Properties

        private ConnectionViewModel _selectedConnection;

        public ConnectionViewModel SelectedConnection
        {
            get => _selectedConnection;
            set => SetProperty(ref _selectedConnection, value);
        }

        private StateViewModel _selectedState;

        public StateViewModel SelectedState
        {
            get => _selectedState;
            set
            {
                SetProperty(ref _selectedState, value);
                Refresh();
            }
        }

        private StateNetworkViewModel _stateNetwork;

        public StateNetworkViewModel StateNetwork
        {
            get => _stateNetwork;
            set => SetProperty(ref _stateNetwork, value);
        }


        public ObservableCollection<StateViewModel> States { get; set; } = new ObservableCollection<StateViewModel>();
        public ObservableCollection<InputViewModel> Inputs { get; set; } = new ObservableCollection<InputViewModel>();

        #endregion

        #region Commands

        private DelegateCommand _deleteCommand;

        public DelegateCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new DelegateCommand(() =>
            {
                if (SelectedState == null) return;

                SelectedState.DeleteConnection(SelectedConnection);

                OnConnectionModified?.Invoke(this, SelectedConnection);
            }));


        private DelegateCommand _updateCommand;

        public DelegateCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new DelegateCommand(() => { }));

        private DelegateCommand _createCommand;

        public DelegateCommand CreateCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(() =>
            {
                if (SelectedState == null) return;

                SelectedState.CreateConnection();

                OnConnectionModified?.Invoke(this, SelectedConnection);
            }));

        #endregion
    }
}