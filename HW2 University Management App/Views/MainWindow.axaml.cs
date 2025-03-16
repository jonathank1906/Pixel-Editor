using Avalonia.Controls;

namespace HW2_University_Management_App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
    private async void OpenDialog(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new DialogWindow();
        await dialog.ShowDialog(this); // Opens the dialog as a modal window
    }
}