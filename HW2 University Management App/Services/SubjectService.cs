using System.Collections.Generic;
using System.Linq;
using HW2_University_Management_App.Models;

namespace HW2_University_Management_App.Services
{
    public class SubjectService
    {
        // Simulating a database with a static list of subjects
        private static List<Subject> subjects = new List<Subject>
        {
            new Subject { SubjectID = "101", Name = "Math", TeacherID = "sarah" },
            new Subject { SubjectID = "102", Name = "Physics", TeacherID = "sarah" },
            new Subject { SubjectID = "103", Name = "Biology", TeacherID = "sarah" },
            new Subject { SubjectID = "104", Name = "Chemistry", TeacherID = "sarah" },
            new Subject { SubjectID = "105", Name = "Sports", TeacherID = "sarah" },
            new Subject { SubjectID = "106", Name = "Computer", TeacherID = "sarah" },
            new Subject { SubjectID = "107", Name = "Spanish", TeacherID = "sarah" },
            new Subject { SubjectID = "108", Name = "JavaScript", TeacherID = "sarah" },
            new Subject { SubjectID = "109", Name = "Python", TeacherID = "sarah" },
            new Subject { SubjectID = "110", Name = "HTML", TeacherID = "sarah" },
        };

        public List<Subject> GetAvailableSubjects() => subjects;

        public List<Subject> GetSubjectsByTeacher(string teacherId)
        {
            return subjects.Where(s => s.TeacherID == teacherId).ToList();
        }

        public void EnrollStudent(string studentId, string subjectId)
        {
            var subject = subjects.FirstOrDefault(s => s.SubjectID == subjectId);
            if (subject != null && !subject.EnrolledStudents.Contains(studentId))
            {
                subject.EnrolledStudents.Add(studentId);
            }
        }

        public void DropStudent(string studentId, string subjectId)
        {
            var subject = subjects.FirstOrDefault(s => s.SubjectID == subjectId);
            if (subject != null)
            {
                subject.EnrolledStudents.Remove(studentId);
            }
        }

        public void CreateSubject(string name, string teacherId)
        {
            string newId = (subjects.Count + 101).ToString(); // Generate a simple ID
            subjects.Add(new Subject { SubjectID = newId, Name = name, TeacherID = teacherId });
        }

        public void DeleteSubject(string subjectId)
        {
            subjects.RemoveAll(s => s.SubjectID == subjectId);
        }
    }
}
