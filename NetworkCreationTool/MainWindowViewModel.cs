using System;
using System.Windows.Forms;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
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

        public void New()
        {
            _selectedFilePath = string.Empty;
            _network = new StateNetwork();
            var startState = _network.CreateState("Start");
            _network.StartState = startState;
            StateNetworkViewModel.Network = _network;
            StateEditorViewModel.Network = _network;
        }

        public void Load()
        {
            _selectedFilePath = SelectFile();
            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                return;
            }

            _network = new StateNetworkJsonSerializer(_selectedFilePath).Read();
            StateNetworkViewModel.Network = _network;
            StateEditorViewModel.Network = _network;
        }

        public void Save()
        {
            while (string.IsNullOrEmpty(_selectedFilePath))
            {
                _selectedFilePath = SaveNew();
            }

            new StateNetworkJsonSerializer(_selectedFilePath).Write(_network);
        }

        public string SaveNew()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "NewNetwork"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "Json Files (*.json) |*.json;"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            return result == true ? dlg.FileName : string.Empty;
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
       
        private DelegateCommand _newButtonCommand;

        public DelegateCommand NewButtonCommand =>
            _newButtonCommand ?? (_newButtonCommand = new DelegateCommand(New));

        
        #endregion
    }
}