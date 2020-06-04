using Aptacode.StateNet.Network;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.Wpf.ViewModels
{
    public class InputSelectorViewModel : BindableBase
    {
        public InputSelectorViewModel(StateNetworkViewModel stateNetworkViewModel)
        {
            StateNetwork = stateNetworkViewModel;
        }

        #region Methods

        #endregion

        #region Properties

        private InputViewModel _selectedInput;

        public InputViewModel SelectedInput
        {
            get => _selectedInput;
            set => SetProperty(ref _selectedInput, value);
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
                if (SelectedInput == null) return;

                StateNetwork.Delete(SelectedInput.Model);
            }));


        private DelegateCommand _updateCommand;

        public DelegateCommand UpdateCommand =>
            _updateCommand ?? (_updateCommand = new DelegateCommand(() => { }));

        private DelegateCommand _createCommand;

        public DelegateCommand CreateCommand =>
            _createCommand ?? (_createCommand = new DelegateCommand(() =>
            {
                StateNetwork.Add(new Input($"Input {StateNetwork.Inputs.Count + 1}"));
            }));

        #endregion
    }
}