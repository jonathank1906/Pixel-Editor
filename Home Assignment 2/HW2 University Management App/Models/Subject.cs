using System.Collections.Generic;

namespace HW2_University_Management_App.Models;
public class Subject
{
    public string SubjectID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } // 🔹 New field for subject description
    public string TeacherID { get; set; }  // The teacher who created the subject
    public List<string> EnrolledStudents { get; set; } = new(); // List of student usernames
}