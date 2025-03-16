using Avalonia.Controls;
using Avalonia.Interactivity;

namespace HW2_University_Management_App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void OpenDialog(object? sender, RoutedEventArgs e)
        {
            // Define a message to pass to the DialogWindow
            string message = "This is your notification message!";
            
            // Create the dialog, passing the message to the constructor
            var dialog = new DialogWindow(message);

            // Show the dialog as a modal window and handle the result if needed
            var result = await dialog.ShowDialog<bool>(this);
        }
    }
}
