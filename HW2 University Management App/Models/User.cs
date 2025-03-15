using System;
using System.Collections.Generic;

namespace HW2_University_Management_App.Models
{
    public class User
    {
        public string? UserID { get; set; }
        public string? UserPassword { get; set; }
        public string? UserRole { get; set; }

        public List<string> EnrolledSubjects { get; set; } = new(); // For Students
        public List<string> CreatedSubjects { get; set; } = new(); // For Teachers


    }




}
