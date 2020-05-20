using System;
using Aptacode.StateNet.Network;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.Wpf.ViewModels
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
            _deleteCommand ?? (_deleteCommand = new DelegateCommand(() =>
            {
                if (SelectedState == null)
                {
                    return;
                }

                StateNetwork.Delete(SelectedState.Model);
            }));


        private DelegateCommand<string> _updateCommand;

        public DelegateCommand<string> UpdateCommand =>
            _updateCommand ?? (_updateCommand = new DelegateCommand<string>(newName =>
            {
                var alteredState = SelectedState;
                StateNetwork.Clear();

                alteredState.Model.Name = newName;
                alteredState.Name = newName;

                StateNetwork.Load();
                OnStateRenamed?.Invoke(this, alteredState);
            }));

        private DelegateCommand _createCommand;

        public DelegateCommand CreateCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(() =>
            {
                StateNetwork.Add(new State($"State {StateNetwork.States.Count + 1}"));
            }));

        #endregion
    }
}