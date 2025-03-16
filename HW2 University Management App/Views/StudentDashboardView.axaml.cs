using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Avalonia.Interactivity;


namespace HW2_University_Management_App.Views
{
   public partial class StudentDashboardView : UserControl
{
    public StudentDashboardView()
    {
        InitializeComponent();
    }

    private async void OpenDialog(object? sender, RoutedEventArgs e)
    {
        // Get the message from the ViewModel
        string courseName = "success";

        // Create and show the dialog with the course name message
        var dialog = new DialogWindow(courseName);
        var result = await dialog.ShowDialog<bool>(GetWindow());
    }

    private Window GetWindow()
    {
        return (Window)this.VisualRoot;
    }
}

}
