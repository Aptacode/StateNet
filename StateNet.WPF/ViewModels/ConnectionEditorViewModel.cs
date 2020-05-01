﻿using System;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class ConnectionEditorViewModel : BindableBase
    {
        public ConnectionEditorViewModel(StateNetworkViewModel stateNetworkViewModel)
        {
            StateNetwork = stateNetworkViewModel;
        }

        #region Events

        public event EventHandler<ConnectionViewModel> OnConnectionModified;

        #endregion

        #region Methods

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
            set => SetProperty(ref _selectedState, value);
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

                OnConnectionModified?.Invoke(this, SelectedConnection);
            }));


        private DelegateCommand _updateCommand;

        public DelegateCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new DelegateCommand(() => { }));

        private DelegateCommand _createCommand;

        public DelegateCommand CreateCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(async () =>
            {
                if (SelectedState == null)
                {
                    return;
                }

                SelectedState.CreateConnection();

                OnConnectionModified?.Invoke(this, SelectedConnection);
            }));

        #endregion
    }
}