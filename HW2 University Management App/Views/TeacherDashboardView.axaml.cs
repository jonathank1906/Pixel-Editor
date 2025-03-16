using Avalonia.Controls;
using Avalonia.Interactivity;


namespace HW2_University_Management_App.Views
{
    public partial class TeacherDashboardView : UserControl
    {
        public TeacherDashboardView()
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