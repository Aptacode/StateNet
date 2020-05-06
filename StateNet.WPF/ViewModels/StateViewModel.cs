using System;
using System.Collections.ObjectModel;
using System.Linq;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;
using Prism.Mvvm;

namespace Aptacode.StateNet.WPF.ViewModels
{
    public class StateViewModel : BindableBase, IEquatable<StateViewModel>
    {
        public StateViewModel(State model) : this(model, false)
        {
        }

        public StateViewModel(State model, bool loadConnections)
        {
            Connections = new ObservableCollection<ConnectionViewModel>();
            Model = model;
            LoadConnections = loadConnections;
        }

        #region Methods

        public void Load()
        {
            Connections.Clear();

            if (_model == null)
            {
                Name = string.Empty;
                return;
            }

            Name = _model.Name;

            if (_loadConnections)
            {
                Connections.AddRange(_model.Connections.Select(c => new ConnectionViewModel(c)));
            }
        }

        #endregion

        public void CreateConnection()
        {
            Model.Add(new Connection(Model, null, Model, new ConnectionWeight(1)));
            Load();
        }

        #region Properties

        private bool _loadConnections;

        public bool LoadConnections
        {
            get => _loadConnections;
            set
            {
                SetProperty(ref _loadConnections, value);
                Load();
            }
        }

        private State _model;

        public State Model
        {
            get => _model;
            set
            {
                SetProperty(ref _model, value);
                Load();
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);

                if (Model != null)
                {
                    Model.Name = _name;
                }
            }
        }

        public ObservableCollection<ConnectionViewModel> Connections { get; set; }

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return Model?.GetHashCode() ?? base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is StateViewModel other && Equals(other);
        }

        public bool Equals(StateViewModel other)
        {
            return other != null && GetHashCode() == other.GetHashCode();
        }

        #endregion Equality
    }
}