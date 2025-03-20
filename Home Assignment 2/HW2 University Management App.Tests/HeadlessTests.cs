using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Xunit;
using System.Threading.Tasks;
using Avalonia.Input;

using HW2_University_Management_App.Views;
using HW2_University_Management_App.ViewModels;
using HW2_University_Management_App.Models;

namespace HW2_University_Management_App.Tests
{
    public class HeadlessTests
    {
        [AvaloniaFact]
        public async Task ButtonClick_SetsSignInSucceedToTrue()
        {
            // Arrange
            var loginWindow = new LoginWindow();
            var loginViewModel = new LoginWindowViewModel(loginWindow);
            loginWindow.DataContext = loginViewModel;
            loginWindow.Show();

            await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Loaded);
            await Task.Delay(100); // Reduced delay, but still ensure UI is ready

            // Find controls
            var usernameTextBox = loginWindow.FindControl<TextBox>("UsernameTextBox");
            var passwordBox = loginWindow.FindControl<TextBox>("PasswordBox");
            var loginButton = loginWindow.FindControl<Button>("LoginButton");

            Assert.NotNull(usernameTextBox);
            Assert.NotNull(passwordBox);
            Assert.NotNull(loginButton);

            // Set text directly and raise property changed events
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                usernameTextBox.Text = "jake";
                passwordBox.Text = "student123";
                
                // Ensure view model properties are updated
                loginViewModel.Username = usernameTextBox.Text;
                loginViewModel.Password = passwordBox.Text;
            });

            // Click login button
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                loginButton.Command?.Execute(null);
            });

            // Wait for async login operation to complete
            int attempts = 0;
            const int maxAttempts = 50; // Increased max attempts
            const int delayMs = 100; // Shorter delay between attempts
            
            while (!loginViewModel.SignInSucceed && attempts < maxAttempts)
            {
                await Task.Delay(delayMs);
                attempts++;
            }

            // Assert
            Assert.True(loginViewModel.SignInSucceed, 
                $"Login should have succeeded after {attempts} attempts. Username: {loginViewModel.Username}, Password: {loginViewModel.Password}");
        }
    }
}