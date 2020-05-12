using System;
using Aptacode.StateNet.Network;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateSelectorViewModel : BindableBase
    {
        public StateSelectorViewModel(StateNetworkViewModel stateNetworkViewModel)
        {
            StateNetwork = stateNetworkViewModel;
        }

        #region Events

        public EventHandler<StateViewModel> OnStateSelected { get; set; }
        public EventHandler<StateViewModel> OnStateRenamed { get; set; }

        #endregion

        #region Methods

        #endregion

        #region Properties

        private StateViewModel _selectedState;

        public StateViewModel SelectedState
        {
            get => _selectedState;
            set
            {
                SetProperty(ref _selectedState, value);
                OnStateSelected?.Invoke(this, _selectedState);
            }
        }

        private StateNetworkViewModel _stateNetwork;

        public StateNetworkViewModel StateNetwork
        {
            get => _stateNetwork;
            set => SetProperty(ref _stateNetwork, value);
        }

        #endregion

        #region Commands

        private DelegateCommand _deleteCommand;

        public DelegateCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new DelegateCommand(async () =>
            {
                if (SelectedState == null)
                {
                    return;
                }

                StateNetwork.Delete(SelectedState.Model);
            }));


        private DelegateCommand _updateCommand;

        public DelegateCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new DelegateCommand(() =>
            {
                OnStateRenamed?.Invoke(this, SelectedState);
                SelectedState = null;
                StateNetwork.Load();
            }));

        private DelegateCommand _createCommand;

        public DelegateCommand CreateCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(async () =>
            {
                StateNetwork.Add(new State($"State {StateNetwork.States.Count + 1}"));
            }));

        #endregion
    }
}