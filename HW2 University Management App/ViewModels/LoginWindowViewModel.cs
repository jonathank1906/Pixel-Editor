using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Views;
using HW2_University_Management_App.Services;
using System.Collections.Generic;
using System.Diagnostics;

namespace HW2_University_Management_App.ViewModels
{
    public partial class LoginWindowViewModel : ViewModelBase
    {
        private readonly Window closeable;
        private readonly SubjectService subjectService;
        
        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string username = "";

        [ObservableProperty]
        private bool signInSucceed = false;

        // Constructor initializes SubjectService (JSON-based data)
        public LoginWindowViewModel(Window closeable)
        {
            this.closeable = closeable;
            subjectService = new SubjectService(); // Load users from JSON
        }

        [RelayCommand]
        public void AttemptLogin()
        {
            User user = subjectService.AuthenticateUser(Username, Password);

            if (user != null)
            {
                SignInSucceed = true;
                Debug.WriteLine("Login successful");
                OpenMainWindow(user);
            }
            else
            {
                Debug.WriteLine("Login failed: Invalid username or password");
            }
        }

        private void OpenMainWindow(User user)
        {
            var mainWindow = new MainWindow();
            var mainWindowViewModel = new MainWindowViewModel(user, mainWindow);
            mainWindow.DataContext = mainWindowViewModel;

            mainWindow.Show();
            closeable.Close(); // Close login window
        }
    }
}
