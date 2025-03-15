using CommunityToolkit.Mvvm.ComponentModel;
using HW2_University_Management_App.Models;
using System.Collections.ObjectModel;
using System;

namespace HW2_University_Management_App.ViewModels
{
    public partial class StudentDashboardViewModel : ViewModelBase
    {
        public ObservableCollection<string> EnrolledSubjects { get; set; }

        public StudentDashboardViewModel(User student)
        {
            EnrolledSubjects = new ObservableCollection<string>(student.EnrolledSubjects);
        }
    }
}
