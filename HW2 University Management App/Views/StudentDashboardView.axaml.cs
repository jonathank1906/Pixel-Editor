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
            var dialog = new DialogWindow();
            var result = await dialog.ShowDialog<bool>(GetWindow());
            // Handle the result if needed
        }

        private Window GetWindow()
        {
            return (Window)this.VisualRoot;
        }
    }
}
