using System;
using Aptacode.StateNet.Network;
using Prism.Mvvm;

namespace Aptacode.StateNet.Wpf.ViewModels
{
    public class InputViewModel : BindableBase, IEquatable<InputViewModel>
    {
        public InputViewModel(Input model)
        {
            Model = model;
        }

        #region Methods

        #endregion

        #region Properties

        private Input _model;

        public Input Model
        {
            get => _model;
            set
            {
                SetProperty(ref _model, value);
                Name = _model == null ? string.Empty : _model.Name;
            }
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                if (Model != null) Model.Name = _name;
            }
        }

        #endregion

        #region Equality

        public override int GetHashCode()
        {
            return Model?.GetHashCode() ?? base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is InputViewModel other && Equals(other);
        }

        public bool Equals(InputViewModel other)
        {
            return other != null && GetHashCode() == other.GetHashCode();
        }

        #endregion Equality
    }
}