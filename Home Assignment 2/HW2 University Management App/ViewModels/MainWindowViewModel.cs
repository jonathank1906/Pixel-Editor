using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Views;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace HW2_University_Management_App.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public Window window;

    public User userName;
    private readonly SubjectService subjectService;

    [ObservableProperty]
    private object? currentContent;

    [RelayCommand]
    private void logout()
    {

        var loginWindow = new LoginWindow();
        loginWindow.DataContext = new LoginWindowViewModel(loginWindow); // Passes the window so it can be manipulated

        loginWindow.Show();
        window.Close();
    }

    public MainWindowViewModel(User item, Window window)
    {
        this.window = window;
        userName = item;
        subjectService = new SubjectService();

        if (userName.UserRole == "Student")
        {
            CurrentContent = new StudentDashboardView() { DataContext = new StudentDashboardViewModel(userName) };
        }
        else if (userName.UserRole == "Teacher")
        {
            CurrentContent = new TeacherDashboardView() { DataContext = new TeacherDashboardViewModel(userName) };
        }
    }
}