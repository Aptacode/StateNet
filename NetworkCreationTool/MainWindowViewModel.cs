using System.Windows.Forms;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Persistence.Json;
using Aptacode.StateNet.WPF.ViewModels;
using Prism.Commands;
using Prism.Mvvm;

namespace Aptacode.StateNet.NetworkCreationTool
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            StateNetworkViewModel = new StateNetworkViewModel();
            StateNetworkViewModel.OnStateSelected += (s, e) => { StateEditorViewModel.State = e.State; };
            StateEditorViewModel = new StateEditorViewModel();
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
            _selectedFilePath = SelectFile();
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                return;
            }

            _stateNetworkSerializer = new StateNetworkJsonSerializer(_selectedFilePath);
            _network = _stateNetworkSerializer.Read();
            StateNetworkViewModel.Network = _network;
            StateEditorViewModel.Network = _network;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                return;
            }

            _stateNetworkSerializer.Write(_network);
        }

        public string SelectFile()
        {
            var fileDialog = new OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "Json Files (*.json) |*.json;"
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                return fileDialog.FileName;
            }

            return string.Empty;
        }

        #region Properties

        private IStateNetwork _network;
        private StateNetworkJsonSerializer _stateNetworkSerializer;
        private StateEditorViewModel _stateEditorViewModel;
        private string _selectedFilePath;

        private StateNetworkViewModel _stateNetworkViewModel;

        #endregion

        #region Commands

        private DelegateCommand _loadButtonCommand;

        public DelegateCommand LoadButtonCommand =>
            _loadButtonCommand ?? (_loadButtonCommand = new DelegateCommand(Load));

        private DelegateCommand _saveButtonCommand;

        public DelegateCommand SaveButtonCommand =>
            _saveButtonCommand ?? (_saveButtonCommand = new DelegateCommand(Save));

        #endregion
    }
}