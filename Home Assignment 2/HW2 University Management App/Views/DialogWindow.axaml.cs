using Avalonia.Controls;
using HW2_University_Management_App.ViewModels;


namespace HW2_University_Management_App.Views;

public partial class DialogWindow : Window
{
    public DialogWindow(string message)
    {
        InitializeComponent();
        DataContext = new DialogWindowViewModel(message);
    }

    private void Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(true); // Return 'true' when OK is clicked
    }

    private void Cancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(false); // Return 'false' when Cancel is clicked
    }
}

