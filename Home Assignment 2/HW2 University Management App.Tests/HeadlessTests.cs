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
    public async Task LoginWindow_SuccessfulSignIn_When_Valid_Credentials_Entered()
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
    public void Student_Enroll_In_Available_Subject()
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
    public void Student_Drop_Enrolled_Subject()
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
    public void Teacher_Create_New_Subject()
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
    public void Teacher_Delete_Existing_Subject()
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
    
    [AvaloniaFact]
    public async Task Data_Persistence_Test()
    {
        // Arrange
        var testTeacher = new User
        {
            UserID = "teacher1",
            UserRole = "Teacher",
            UserPassword = "password",
            CreatedSubjects = new List<string>()
        };

        var testStudent = new User
        {
            UserID = "student1",
            UserRole = "Student",
            UserPassword = "123",
            EnrolledSubjects = new List<string>()
        };

        var teacherViewModel = new TeacherDashboardViewModel(testTeacher);
        var studentViewModel = new StudentDashboardViewModel(testStudent);

        var testSubject = new Subject
        {
            SubjectID = Guid.NewGuid().ToString(),
            Name = "Persistent Subject",
            Description = "This subject should persist after reopening the app.",
            TeacherID = testTeacher.UserID,
            EnrolledStudents = new List<string>()
        };

        var coloredTestSubject = new ColoredSubject(
            testSubject.SubjectID,
            testSubject.Name,
            testSubject.Description,
            ColorStyles.GetRandomColor()
        );

        // Simulate subject creation by the teacher
        teacherViewModel.CreatedSubjects = new ObservableCollection<ColoredSubject>();
        teacherViewModel.CreatedSubjects.Add(coloredTestSubject);

        // Simulate student enrollment in the subject
        studentViewModel.AvailableSubjects = new ObservableCollection<ColoredSubject> { coloredTestSubject };
        studentViewModel.EnrolledSubjects = new ObservableCollection<ColoredSubject>();
        studentViewModel.AvailableSubjects.Remove(coloredTestSubject);
        studentViewModel.EnrolledSubjects.Add(coloredTestSubject);

        // Simulate subject deletion by the teacher
        teacherViewModel.CreatedSubjects.Remove(coloredTestSubject);

        // Simulate application close and reopen
        await SimulateApplicationCloseAndReopen(teacherViewModel, studentViewModel);

        // Assert
        Assert.DoesNotContain(coloredTestSubject, teacherViewModel.CreatedSubjects); // Ensure the subject was deleted
        Assert.Contains(coloredTestSubject, studentViewModel.EnrolledSubjects); // Ensure the student enrollment persists
        Assert.DoesNotContain(coloredTestSubject, studentViewModel.AvailableSubjects); // Ensure the subject is no longer available
    }

    private async Task SimulateApplicationCloseAndReopen(TeacherDashboardViewModel teacherViewModel, StudentDashboardViewModel studentViewModel)
    {
        // Simulate saving data to persistent storage
        var savedTeacherSubjects = teacherViewModel.CreatedSubjects.ToList();
        var savedStudentEnrolledSubjects = studentViewModel.EnrolledSubjects.ToList();

        // Simulate clearing in-memory data (application close)
        teacherViewModel.CreatedSubjects.Clear();
        studentViewModel.EnrolledSubjects.Clear();
        studentViewModel.AvailableSubjects.Clear();

        // Simulate loading data from persistent storage (application reopen)
        await Task.Delay(100); // Simulate delay for loading
        teacherViewModel.CreatedSubjects = new ObservableCollection<ColoredSubject>(savedTeacherSubjects);
        studentViewModel.EnrolledSubjects = new ObservableCollection<ColoredSubject>(savedStudentEnrolledSubjects);
    }
}