using Avalonia.Controls;
using Avalonia.Interactivity;
using HW2_University_Management_App.ViewModels;

namespace HW2_University_Management_App.Views
{
    public partial class TeacherDashboardView : UserControl
    {
        private TeacherDashboardViewModel ViewModel => (TeacherDashboardViewModel)DataContext;

        public TeacherDashboardView()
        {
            InitializeComponent();
        }

        private async void OpenDialog(object? sender, RoutedEventArgs e)
        {
            string message = string.Empty;

            // Check if the selected subject was created or deleted and set the message accordingly
            if (ViewModel.SelectedExistingSubject != null)
            {
                message = $"Successfully deleted {ViewModel.SelectedExistingSubject.Name}";
            }
            else if (ViewModel.NewlyCreatedSubject != null)
            {
                message = $"Successfully created {ViewModel.NewlyCreatedSubject.Name}";
            }
            else
            {
                message = "Action was not successful. Please try again.";
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
