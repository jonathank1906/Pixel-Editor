using System;
using System.Reactive;
using ReactiveUI;
using Avalonia;
using Avalonia.Interactivity;
using HW2_University_Management_App.Views;

namespace HW2_University_Management_App.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private readonly LoginWindow _loginWindow;

        private string userName = string.Empty;
        private string password = string.Empty;
        private string errorMessage = string.Empty;
        private bool showPassword;

        public LoginWindowViewModel(LoginWindow loginWindow)
        {
            _loginWindow = loginWindow;
            LoginCommand = ReactiveCommand.Create(Login);
        }

        public string UserName
        {
            get => userName;
            set => this.RaiseAndSetIfChanged(ref userName, value);
        }

        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => this.RaiseAndSetIfChanged(ref errorMessage, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        private void Login()
        {
            const string correctUsername = "Danfoss";
            const string correctPassword = "Danfoss";

            if (UserName == correctUsername && Password == correctPassword)
            {
                ErrorMessage = "Login successful!";
                // Logic to switch to the main application view
                // This might involve setting a property or calling a method on the main window
            }
            else
            {
                ErrorMessage = "Invalid username or password. Hint: Danfoss";
            }
        }

        public bool ShowPassword
        {
            get => showPassword;
            set => this.RaiseAndSetIfChanged(ref showPassword, value);
        }
    }
}