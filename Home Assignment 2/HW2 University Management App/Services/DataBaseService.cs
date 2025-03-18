using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HW2_University_Management_App.Models;

namespace HW2_University_Management_App.Services
{
    public class DatabaseService
    {
        private const string FilePath = "UserData.json";
        private DatabaseModel database;

        public DatabaseService()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                database = JsonSerializer.Deserialize<DatabaseModel>(json) ?? new DatabaseModel();
            }
            else
            {
                database = new DatabaseModel();
                SaveDatabase();
            }
        }

        public void SaveDatabase()
        {
            string json = JsonSerializer.Serialize(database, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public List<User> GetUsers() => database.Users;
        public List<Subject> GetSubjects() => database.Subjects;

        public void AddUser(User user)
        {
            database.Users.Add(user);
            SaveDatabase();
        }

        public void AddSubject(Subject subject)
        {
            database.Subjects.Add(subject);
            SaveDatabase();
        }

        public void RemoveSubject(string subjectName)
        {
            database.Subjects.RemoveAll(s => s.Name == subjectName);
            SaveDatabase();
        }

        public User GetUserById(string userId)
        {
            return database.Users.Find(u => u.UserID == userId);
        }
    }

    public class DatabaseModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
