using System;
using ReactiveUI;
using HW2_University_Management_App.Views;

namespace HW2_University_Management_App.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            MainApp = new MainAppViewModel();
            var loginWindow = new LoginWindow();
            Login = new LoginWindowViewModel(loginWindow);
            _contentViewModel = Login;
        }

        public LoginWindowViewModel Login { get; }
        public MainAppViewModel MainApp { get; }
        private ViewModelBase _contentViewModel;

        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public void LoginButtonCommand()
        {
            ContentViewModel = MainApp;
        }
    }
}