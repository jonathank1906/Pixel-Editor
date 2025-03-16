using Avalonia.Controls;
using HW2_University_Management_App.Interfaces;
using HW2_University_Management_App.ViewModels;

namespace HW2_University_Management_App.Views;

public partial class LoginWindow : Window, ICloseble
{
    public LoginWindow()
    {
        InitializeComponent();
         DataContext = new LoginWindowViewModel(this);
    }
}