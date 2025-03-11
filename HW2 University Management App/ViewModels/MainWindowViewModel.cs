using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.ViewModels.AdminMainPage;
using HW2_University_Management_App.Views;
using ReactiveUI;
using System.Collections.ObjectModel;


namespace HW2_University_Management_App.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public Window window;

        public User userName;

        [ObservableProperty]
        private object currentContent;

        [RelayCommand]
        private void logout()
        {

            var loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginWindowViewModel(loginWindow); // Passes the window so it can be manipulated

            loginWindow.Show();
            window.Close();
        }

        [RelayCommand]
        private void GoHome()
        {
            if (userName.UserRole == "Admin")
            {
                window.Width = 800;
                window.Height = 450;
                CurrentContent = new AdminView() { DataContext = new AdminMainPageViewModel(this) };
            }
        }

        public MainWindowViewModel(User item, Window window)
        {
            this.window = window;
            userName = item;
        }
    }
}