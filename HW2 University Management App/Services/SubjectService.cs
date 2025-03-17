using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HW2_University_Management_App.Models;
using System.Diagnostics;

namespace HW2_University_Management_App.Services
{
    public class SubjectService
    {
        private const string DatabaseFile = "UserData.json";
        private List<Subject> subjects;
        private List<User> users;

        public SubjectService()
        {
            LoadData();
        }

        // 🔹 Load Users & Subjects from JSON
        private void LoadData()
        {
            if (File.Exists(DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseFile);
                var database = JsonSerializer.Deserialize<Database>(json);
                subjects = database?.Subjects ?? new List<Subject>();
                users = database?.Users ?? new List<User>();
            }
            else
            {
                subjects = new List<Subject>();
                users = new List<User>();
                SaveData(); // Create JSON if missing
            }
        }

        // 🔹 Save Users & Subjects to JSON
        public void SaveData()
        {
            var database = new Database { Subjects = subjects, Users = users };
            string json = JsonSerializer.Serialize(database, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DatabaseFile, json);
        }

        // 🔹 Retrieve Users
        public List<User> GetUsers() => users;

        // 🔹 Retrieve Subjects
        public List<Subject> GetSubjects() => subjects;

        // 🔹 Authenticate Users
        public User AuthenticateUser(string username, string password)
        {
            Debug.WriteLine($"Authenticating user: {username}, password: {password}");
            return users.FirstOrDefault(u => u.UserID == username && u.UserPassword == password);
        }

        // 🔹 Enroll Student in Subject
        public bool EnrollStudent(string studentId, string subjectId)
        {
            var student = users.FirstOrDefault(u => u.UserID == studentId && u.UserRole == "Student");
            var subject = subjects.FirstOrDefault(s => s.SubjectID == subjectId);

            if (student != null && subject != null && !subject.EnrolledStudents.Contains(studentId))
            {
                subject.EnrolledStudents.Add(studentId);
                student.EnrolledSubjects.Add(subjectId);
                SaveData();
                return true;
            }
            return false;
        }

        // 🔹 Remove Student from Subject
        public bool DropStudent(string studentId, string subjectId)
        {
            var student = users.FirstOrDefault(u => u.UserID == studentId && u.UserRole == "Student");
            var subject = subjects.FirstOrDefault(s => s.SubjectID == subjectId);

            if (student != null && subject != null && subject.EnrolledStudents.Contains(studentId))
            {
                subject.EnrolledStudents.Remove(studentId);
                student.EnrolledSubjects.Remove(subjectId);
                SaveData();
                return true;
            }
            return false;
        }

        // 🔹 Create a New Subject
       public string CreateSubject(string name, string teacherId)
{
    string newId = Guid.NewGuid().ToString(); // Generate ID inside service

    var newSubject = new Subject 
    { 
        SubjectID = newId, 
        Name = name, 
        TeacherID = teacherId, 
        EnrolledStudents = new List<string>() 
    };

    subjects.Add(newSubject);

    var teacher = users.FirstOrDefault(u => u.UserID == teacherId && u.UserRole == "Teacher");
    if (teacher != null)
    {
        teacher.CreatedSubjects.Add(newId);
    }

    SaveData();
    return newId;  // Return the correct ID
}



        // 🔹 Delete a Subject
        // 🔹 Delete a Subject (Updated)
        public void DeleteSubject(string subjectId)
        {
            Debug.WriteLine($"Attempting to delete subject: {subjectId}");

            // 🔹 Find subject in memory
            var subjectToRemove = subjects.FirstOrDefault(s => s.SubjectID == subjectId);

            if (subjectToRemove != null)
            {
                subjects.Remove(subjectToRemove);

                foreach (var student in users.Where(u => u.UserRole == "Student"))
                {
                    student.EnrolledSubjects.Remove(subjectId);
                }

                var teacher = users.FirstOrDefault(u => u.UserRole == "Teacher" && u.CreatedSubjects.Contains(subjectId));
                if (teacher != null)
                {
                    teacher.CreatedSubjects.Remove(subjectId);
                }

                SaveData();
                LoadData(); // 🔥 Force reload to ensure the deletion is reflected properly

                Debug.WriteLine($"Deleted Subject: {subjectId}");
            }
            else
            {
                Debug.WriteLine($"Subject NOT found for deletion: {subjectId}");
            }
        }

    }

    public class Database
    {
        public List<User> Users { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
    }
}
