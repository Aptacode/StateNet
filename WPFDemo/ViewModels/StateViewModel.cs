namespace WPFDemo.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
		private bool _IsActive;

		public bool IsActive
		{
			get { return _IsActive; }
            set { SetField(ref _IsActive, value); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { SetField(ref _Name, value); }
        }

	}
}
