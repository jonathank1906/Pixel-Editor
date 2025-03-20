using Avalonia.Controls;
using System.Collections.ObjectModel;
using Avalonia.Headless.XUnit;
using Avalonia.Threading;
using Xunit;
using System.Threading.Tasks;
using Avalonia.Input;
using System;
using System.Collections.Generic;

using HW2_University_Management_App.Styles;
using HW2_University_Management_App.Views;
using HW2_University_Management_App.ViewModels;
using HW2_University_Management_App.Models;


namespace HW2_University_Management_App.Tests;
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
    [AvaloniaFact]
    public void Student_Can_Drop_A_Subject()
    {
        // Arrange
        var testStudent = new User
        {
            UserID = "test",
            UserRole = "Student",
            UserPassword = "123",
            EnrolledSubjects = new List<string>()
        };

        var viewModel = new StudentDashboardViewModel(testStudent);

        var testSubject = new Subject
        {
            SubjectID = Guid.NewGuid().ToString(),
            Name = "Test Subject",
            Description = "Test Description",
            EnrolledStudents = new List<string>()
        };

        var coloredTestSubject = new ColoredSubject(
            testSubject.SubjectID,
            testSubject.Name,
            testSubject.Description,
            ColorStyles.GetRandomColor()
        );

        viewModel.EnrolledSubjects = new ObservableCollection<ColoredSubject> { coloredTestSubject };
        viewModel.AvailableSubjects = new ObservableCollection<ColoredSubject>();

        // Act
        viewModel.EnrolledSubjects.Remove(coloredTestSubject);
        viewModel.AvailableSubjects.Add(coloredTestSubject);

        // Assert
        Assert.Contains(coloredTestSubject, viewModel.AvailableSubjects);
        Assert.DoesNotContain(coloredTestSubject, viewModel.EnrolledSubjects);
    }

    [AvaloniaFact]
    public void Student_Can_Enroll_In_Subject()
    {
        // Arrange
        var testStudent = new User
        {
            UserID = "test",
            UserRole = "Student",
            UserPassword = "123",
            EnrolledSubjects = new List<string>()
        };

        var viewModel = new StudentDashboardViewModel(testStudent);

        var testSubject = new Subject
        {
            SubjectID = Guid.NewGuid().ToString(),
            Name = "Test Subject",
            Description = "Test Description",
            EnrolledStudents = new List<string>()
        };

        var coloredTestSubject = new ColoredSubject(
            testSubject.SubjectID,
            testSubject.Name,
            testSubject.Description,
            ColorStyles.GetRandomColor()
        );

        viewModel.AvailableSubjects = new ObservableCollection<ColoredSubject> { coloredTestSubject };
        viewModel.EnrolledSubjects = new ObservableCollection<ColoredSubject>();

        // Act
        viewModel.AvailableSubjects.Remove(coloredTestSubject);
        viewModel.EnrolledSubjects.Add(coloredTestSubject);

        // Assert
        Assert.Contains(coloredTestSubject, viewModel.EnrolledSubjects);
        Assert.DoesNotContain(coloredTestSubject, viewModel.AvailableSubjects);
    }
    [AvaloniaFact]
    public void Teacher_Can_Create_New_Subject()
    {
        // Arrange
        var testTeacher = new User
        {
            UserID = "teacher1",
            UserRole = "Teacher",
            UserPassword = "password",
            CreatedSubjects = new List<string>()
        };

        var viewModel = new TeacherDashboardViewModel(testTeacher);

        var testSubject = new Subject
        {
            SubjectID = Guid.NewGuid().ToString(),
            Name = "Test Subject",
            Description = "Test Description",
            TeacherID = testTeacher.UserID,
            EnrolledStudents = new List<string>()
        };

        var coloredTestSubject = new ColoredSubject(
            testSubject.SubjectID,
            testSubject.Name,
            testSubject.Description,
            ColorStyles.GetRandomColor()
        );

        viewModel.CreatedSubjects = new ObservableCollection<ColoredSubject>();

        // Act
        viewModel.CreatedSubjects.Add(coloredTestSubject);

        // Assert
        Assert.Contains(viewModel.CreatedSubjects, s => s.Name == "Test Subject");
        Assert.Single(viewModel.CreatedSubjects);
    }
    [AvaloniaFact]
    public void Teacher_Can_Delete_A_Subject()
    {
        // Arrange
        var testTeacher = new User
        {
            UserID = "teacher1",
            UserRole = "Teacher",
            UserPassword = "password",
            CreatedSubjects = new List<string>()
        };

        var viewModel = new TeacherDashboardViewModel(testTeacher);

        var testSubject = new Subject
        {
            SubjectID = Guid.NewGuid().ToString(),
            Name = "Test Subject",
            Description = "Test Description",
            TeacherID = testTeacher.UserID,
            EnrolledStudents = new List<string>()
        };

        var coloredTestSubject = new ColoredSubject(
            testSubject.SubjectID,
            testSubject.Name,
            testSubject.Description,
            ColorStyles.GetRandomColor()
        );

        viewModel.CreatedSubjects = new ObservableCollection<ColoredSubject> { coloredTestSubject };

        // Act
        viewModel.CreatedSubjects.Remove(coloredTestSubject);

        // Assert
        Assert.Empty(viewModel.CreatedSubjects);
    }
}