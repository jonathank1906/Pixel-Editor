using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Views;
using HW2_University_Management_App.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HW2_University_Management_App.ViewModels;
public partial class LoginWindowViewModel : ViewModelBase
{
    private readonly Window closeable;
    private readonly SubjectService subjectService;

    private string errorMessage = string.Empty;

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

    public string ErrorMessage
    {
        get => errorMessage;
        set => this.SetProperty(ref errorMessage, value);
    }

    [RelayCommand]
    public async Task AttemptLogin()
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
            ErrorMessage = "Invalid username or password";
            await ClearErrorMessageAfterDelay();
        }
    }

    private async Task ClearErrorMessageAfterDelay()
    {
        await Task.Delay(2500);
        ErrorMessage = string.Empty;
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