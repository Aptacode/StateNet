using Aptacode.StateNet.Network.Connections;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class ConnectionViewModel : BindableBase
    {
        public ConnectionViewModel(Connection model)
        {
            Model = model;
        }

        #region Methods

        public void Load()
        {
            SourceViewModel = new StateViewModel(_model.Source, false);
            InputViewModel = new InputViewModel(_model.Input);
            TargetViewModel = new StateViewModel(_model.Target, false);
            ConnectionWeight = _model.ConnectionWeight.Expression;
        }

        #endregion

        #region Properties

        private Connection _model;

        public Connection Model
        {
            get => _model;
            set
            {
                SetProperty(ref _model, value);
                Load();
            }
        }


        private StateViewModel _sourceViewModel;

        public StateViewModel SourceViewModel
        {
            get => _sourceViewModel;
            set => SetProperty(ref _sourceViewModel, value);
        }

        private InputViewModel _inputViewModel;

        public InputViewModel InputViewModel
        {
            get => _inputViewModel;
            set
            {
                SetProperty(ref _inputViewModel, value);
                if (Model == null)
                {
                    return;
                }

                Model.Input = _inputViewModel.Model;
            }
        }

        private StateViewModel _targetViewModel;

        public StateViewModel TargetViewModel
        {
            get => _targetViewModel;
            set
            {
                SetProperty(ref _targetViewModel, value);
                if (Model == null)
                {
                    return;
                }

                Model.Target = _targetViewModel.Model;
            }
        }

        private string _connectionWeight;

        public string ConnectionWeight
        {
            get => _connectionWeight;
            set
            {
                SetProperty(ref _connectionWeight, value);
                if (Model == null)
                {
                    return;
                }

                Model.ConnectionWeight = new ConnectionWeight(_connectionWeight);
            }
        }

        #endregion
    }
}