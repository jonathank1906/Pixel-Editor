using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Avalonia.Interactivity;
using HW2_University_Management_App.ViewModels;

namespace HW2_University_Management_App.Views
{
    public partial class StudentDashboardView : UserControl
    {
        private StudentDashboardViewModel ViewModel => (StudentDashboardViewModel)DataContext;
        public StudentDashboardView()
        {
            InitializeComponent();
        }

        private async void OpenDialog(object? sender, RoutedEventArgs e)
        {
            // Get the message from the ViewModel
            string message = string.Empty;
            if (ViewModel.SelectedAvailableSubject != null)
            {
                // If the student successfully enrolled in a subject
                message = $"Successfully enrolled in {ViewModel.SelectedAvailableSubject.Name}";
            }
            else if (ViewModel.SelectedEnrolledSubject != null)
            {
                // If the student successfully dropped a subject
                message = $"Successfully dropped {ViewModel.SelectedEnrolledSubject.Name}";
            }
            else
            {
                message = "Action was not successful.";
            }
            // Create and show the dialog with the course name message
            var dialog = new DialogWindow(message);
            var result = await dialog.ShowDialog<bool>(GetWindow());
        }

        private Window GetWindow()
        {
            return (Window)this.VisualRoot;
        }

        private void ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Deselect the subject by invoking the DeselectSubject method in the ViewModel
            ViewModel.DeselectSubject();
        }

    }

}
