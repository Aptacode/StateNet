using System;
using System.Collections.ObjectModel;
using System.Linq;
using Aptacode.StateNet.Interfaces;
using Aptacode.StateNet.Network;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateNetworkViewModel : BindableBase
    {
        public StateNetworkViewModel(IStateNetwork model)
        {
            Model = model;
        }

        #region Events

        public event EventHandler<IStateNetwork> OnNetworkModified;

        #endregion

        #region Methods

        public void Load()
        {
            Clear();

            if (_model == null)
            {
                return;
            }

            States.AddRange(_model.GetOrderedStates().Select(state => new StateViewModel(state, true)));
            Inputs.AddRange(_model.GetInputs().Select(input => new InputViewModel(input)));

            OnNetworkModified?.Invoke(this, Model);
        }

        public void Delete(Input input)
        {
            Model.RemoveInput(input);
            Load();
        }

        public void Add(Input input)
        {
            Model.CreateInput(input);
            Load();
        }

        public void Delete(State state)
        {
            Model.RemoveState(state);
            Load();
        }

        public void Add(State state)
        {
            Model.CreateState(state);
            Load();
        }

        #endregion

        #region Properties

        public ObservableCollection<StateViewModel> States { get; set; } = new ObservableCollection<StateViewModel>();
        public ObservableCollection<InputViewModel> Inputs { get; set; } = new ObservableCollection<InputViewModel>();

        private IStateNetwork _model;

        public IStateNetwork Model
        {
            get => _model;
            set
            {
                SetProperty(ref _model, value);
                Load();
            }
        }

        #endregion

        public void Clear()
        {
            States.Clear();
            Inputs.Clear();

        }
    }
}