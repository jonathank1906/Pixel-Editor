using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HW2_University_Management_App.ViewModels;

public partial class LoginWindowViewModel : ViewModelBase
{
    private Window closeable;

    [ObservableProperty]
    private string password = "";

    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private bool signInSucceed = false;

    // Hardcoded user data
    private readonly List<User> userData = new List<User>
    {
        new User { UserID = "a", UserPassword = "a" },
        new User { UserID = "user2", UserPassword = "password2" },
        new User { UserID = "jake", UserPassword = "student123", UserRole = "Student", EnrolledSubjects = new List<string> { "Math", "Physics" } },
        new User { UserID = "sarah", UserPassword = "teacher123", UserRole = "Teacher" },
        new User { UserID = "jonathan", UserPassword = "j123", UserRole = "Student", EnrolledSubjects = new List<string> { "Math", "Physics", "Biology", "Sports" } },
        new User { UserID = "azzam", UserPassword = "a123", UserRole = "Teacher", CreatedSubjects = new List<string> { "Math", "Physics" } },
     };
    

    // Create Main window and sets the data context
    Window mainWindow = new MainWindow();
    MainWindowViewModel mainWindowViewModel;

    [RelayCommand]
    private void WrongUsernameOrPassword()
    {
        foreach (var item in userData)
        {
            // checks if the Login was successful
            if (item.UserID == Username)
            {
                if (item.UserPassword == Password)
                {
                    SignInSucceed = true;
                    Debug.WriteLine("Login successful");
                    MainWindowOpen(item);
                    return;
                }
            }
            Debug.WriteLine("Login failed");
            
        }

    }

    public void MainWindowOpen(User item)
    {
        mainWindowViewModel = new MainWindowViewModel(item, mainWindow);
        mainWindow.DataContext = mainWindowViewModel;

        // Closes and opens login page and main window
        mainWindow.Show();
        closeable.Close();
    }

    public LoginWindowViewModel(Window Cloneable)
    {
        // Method to close the login window
        closeable = Cloneable;
    }
};
    