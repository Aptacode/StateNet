namespace WPFDemo.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
        private bool _IsActive;

        private string _Name;

        public bool IsActive
        {
            get => _IsActive;
            set => SetField(ref _IsActive, value);
        }

        public string Name
        {
            get => _Name;
            set => SetField(ref _Name, value);
        }
    }
}