using System.ComponentModel;

namespace HW2_University_Management_App.ViewModels
{
    public class MainAppViewModel : ViewModelBase
    {
        private object? _contentViewModel;

        public MainAppViewModel()
        {
            // Initialize ContentViewModel with an instance of LoginWindowViewModel
            ContentViewModel = new LoginWindowViewModel(new Views.LoginWindow());
        }

        public object? ContentViewModel
        {
            get => _contentViewModel;
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}