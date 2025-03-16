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
            // Determine the course action and set the appropriate message
            string message = string.Empty;
            
            // Example logic based on the sender button's content
            if (sender is Button button)
            {
                if (button.Content.ToString() == "Create Course")
                {
                    message = "Successfully created course!";
                }
                else if (button.Content.ToString() == "Delete Course")
                {
                    message = "Successfully deleted course!";
                }
            }

            // Create the dialog and pass the message
            var dialog = new DialogWindow(message);
            
            // Show the dialog as a modal window
            var result = await dialog.ShowDialog<bool>(GetWindow());
        }

        private Window GetWindow()
        {
            return (Window)this.VisualRoot;
        }
    }
}
