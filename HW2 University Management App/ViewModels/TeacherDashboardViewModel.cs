using CommunityToolkit.Mvvm.ComponentModel;
using HW2_University_Management_App.Models;
using System.Collections.ObjectModel;
using System;

namespace HW2_University_Management_App.ViewModels
{
    public partial class TeacherDashboardViewModel : ViewModelBase
    {
        public ObservableCollection<string> CreatedSubjects { get; set; }

        public TeacherDashboardViewModel(User teacher)
        {
            CreatedSubjects = new ObservableCollection<string>(teacher.CreatedSubjects);
        }
    }
}
