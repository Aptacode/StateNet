using System;
using System.Collections.ObjectModel;
using System.Linq;
using Aptacode.StateNet.Network;
using Aptacode.StateNet.Network.Connections;
using Prism.Mvvm;

namespace Aptacode.StateNet.Wpf.ViewModels
{
    public class StateViewModel : BindableBase, IEquatable<StateViewModel>
    {
        public StateViewModel(State model, bool loadConnections = false)
        {
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

        public void DeleteConnection(ConnectionViewModel selectedConnection)
        {
            if (selectedConnection != null)
            {
                Model.Remove(selectedConnection.Model);
            }

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
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<ConnectionViewModel> Connections { get; set; } =
            new ObservableCollection<ConnectionViewModel>();

        #endregion

        #region Equality

        public override int GetHashCode() => Model?.GetHashCode() ?? base.GetHashCode();

        public override bool Equals(object obj) => obj is StateViewModel other && Equals(other);

        public bool Equals(StateViewModel other) => other != null && GetHashCode() == other.GetHashCode();

        #endregion Equality
    }
}