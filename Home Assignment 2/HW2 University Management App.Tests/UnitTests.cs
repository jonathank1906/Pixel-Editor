using System.IO;
using System.Text.Json;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using Xunit;

namespace HW2_University_Management_App.Tests;
public class SubjectServiceTests
{
    [Fact]
    public void SubjectService_Should_Persist_Changes_Across_Sessions()
    {
        // Arrange
        const string originalFile = "UserData.json";
        const string backupFile = "UserData_Backup.json";

        // Backup the original file if it exists
        if (File.Exists(originalFile))
            File.Copy(originalFile, backupFile, overwrite: true);

        try
        {
            // Create test data
            var initialData = new Database
            {
                Subjects = new List<Subject>
                    {
                        new Subject { SubjectID = "1", Name = "Math", TeacherID = "T1", EnrolledStudents = new List<string> { "S1" } }
                    },
                Users = new List<User>
                    {
                        new User { UserID = "S1", UserRole = "Student", EnrolledSubjects = new List<string> { "1" } },
                        new User { UserID = "T1", UserRole = "Teacher", CreatedSubjects = new List<string> { "1" } }
                    }
            };

            // Write test data to the default file
            File.WriteAllText(originalFile, JsonSerializer.Serialize(initialData, new JsonSerializerOptions { WriteIndented = true }));

            // Act
            var service = new SubjectService();
            service.DropStudent("S1", "1"); // Remove student S1 from subject 1
            service.CreateSubject("History", "World History", "T1"); // Add a new subject

            // Reload service to simulate reopening the application
            var reloadedService = new SubjectService();

            // Assert
            var subjects = reloadedService.GetSubjects();
            var users = reloadedService.GetUsers();

            Assert.DoesNotContain(subjects, s => s.SubjectID == "1" && s.EnrolledStudents.Contains("S1")); // S1 should not be in subject 1
            Assert.Contains(subjects, s => s.Name == "History" && s.Description == "World History"); // History subject should exist
        }
        finally
        {
            // Restore the original file if it was backed up
            if (File.Exists(backupFile))
            {
                File.Copy(backupFile, originalFile, overwrite: true);
                File.Delete(backupFile);
            }
            else
            {
                // Clean up the test file if no backup existed
                if (File.Exists(originalFile))
                    File.Delete(originalFile);
            }
        }
    }

    [Fact]
    public void SubjectService_Can_Create_New_Subject()
    {
        // Arrange
        const string originalFile = "UserData.json";
        const string backupFile = "UserData_Backup.json";

        // Backup the original file if it exists
        if (File.Exists(originalFile))
            File.Copy(originalFile, backupFile, overwrite: true);

        try
        {
            // Create test data
            var initialData = new Database
            {
                Subjects = new List<Subject>(),
                Users = new List<User>
                    {
                        new User { UserID = "T1", UserRole = "Teacher", CreatedSubjects = new List<string>() }
                    }
            };

            // Write test data to the default file
            File.WriteAllText(originalFile, JsonSerializer.Serialize(initialData, new JsonSerializerOptions { WriteIndented = true }));

            // Act
            var service = new SubjectService();
            var newSubjectId = service.CreateSubject("Physics", "Introduction to Physics", "T1");

            // Reload service to simulate reopening the application
            var reloadedService = new SubjectService();

            // Assert
            var subjects = reloadedService.GetSubjects();
            var teacher = reloadedService.GetUsers().FirstOrDefault(u => u.UserID == "T1");

            Assert.NotNull(teacher); // Ensure the teacher exists
            Assert.Contains(subjects, s => s.SubjectID == newSubjectId && s.Name == "Physics" && s.Description == "Introduction to Physics"); // Subject should exist in available subjects
            Assert.Contains(teacher.CreatedSubjects, id => id == newSubjectId); // Subject should appear in teacher's "My Subjects"
        }
        finally
        {
            // Restore the original file if it was backed up
            if (File.Exists(backupFile))
            {
                File.Copy(backupFile, originalFile, overwrite: true);
                File.Delete(backupFile);
            }
            else
            {
                // Clean up the test file if no backup existed
                if (File.Exists(originalFile))
                    File.Delete(originalFile);
            }
        }
    }

    [Fact]
    public void SubjectService_Can_Authenticate_Valid_Users_And_Reject_Invalid_Ones()
    {
        // Arrange
        const string originalFile = "UserData.json";
        const string backupFile = "UserData_Backup.json";

        // Backup the original file if it exists
        if (File.Exists(originalFile))
            File.Copy(originalFile, backupFile, overwrite: true);

        try
        {
            // Create test data
            var initialData = new Database
            {
                Subjects = new List<Subject>(),
                Users = new List<User>
            {
                new User { UserID = "S1", UserRole = "Student", UserPassword = "student123" },
                new User { UserID = "T1", UserRole = "Teacher", UserPassword = "teacher123" }
            }
            };

            // Write test data to the default file
            File.WriteAllText(originalFile, JsonSerializer.Serialize(initialData, new JsonSerializerOptions { WriteIndented = true }));

            // Act
            var service = new SubjectService();

            // Test login for Student
            var student = service.AuthenticateUser("S1", "student123");

            // Test login for Teacher
            var teacher = service.AuthenticateUser("T1", "teacher123");

            // Test invalid login
            var invalidUser = service.AuthenticateUser("S1", "wrongpassword");

            // Assert
            Assert.NotNull(student); // Ensure the student login is successful
            Assert.Equal("S1", student.UserID);
            Assert.Equal("Student", student.UserRole);

            Assert.NotNull(teacher); // Ensure the teacher login is successful
            Assert.Equal("T1", teacher.UserID);
            Assert.Equal("Teacher", teacher.UserRole);

            Assert.Null(invalidUser); // Ensure invalid login fails
        }
        finally
        {
            // Restore the original file if it was backed up
            if (File.Exists(backupFile))
            {
                File.Copy(backupFile, originalFile, overwrite: true);
                File.Delete(backupFile);
            }
            else
            {
                // Clean up the test file if no backup existed
                if (File.Exists(originalFile))
                    File.Delete(originalFile);
            }
        }
    }
}
