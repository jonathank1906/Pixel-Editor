using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System;

namespace HW2_University_Management_App.ViewModels
{
    public partial class TeacherDashboardViewModel : ViewModelBase
    {
        private readonly SubjectService subjectService = new();
        private readonly User teacher;
        private readonly Random random = new();

        public ObservableCollection<ColoredSubject> CreatedSubjects { get; set; }

        [ObservableProperty]
        private string newSubjectName;

        [ObservableProperty]
        private string selectedCreatedSubject;

        public TeacherDashboardViewModel(User teacher)
        {
            this.teacher = teacher;
            CreatedSubjects = new ObservableCollection<ColoredSubject>();

            LoadSubjects();
        }

        private void LoadSubjects()
        {
            CreatedSubjects.Clear();
            foreach (var subject in subjectService.GetSubjectsByTeacher(teacher.UserID))
            {
                var color = ColorStyles.GetRandomColor();
                CreatedSubjects.Add(new ColoredSubject(subject.Name, color));
            }
        }

        [RelayCommand]
        private void CreateSubject()
        {
            if (!string.IsNullOrEmpty(NewSubjectName))
            {
                subjectService.CreateSubject(NewSubjectName, teacher.UserID);
                LoadSubjects(); // Refresh subjects with colors
                NewSubjectName = "";
            }
        }

        [RelayCommand]
        private void DeleteSubject()
        {
            if (!string.IsNullOrEmpty(SelectedCreatedSubject))
            {
                subjectService.DeleteSubject(SelectedCreatedSubject);
               // LoadSubjects(); // Refresh the list
            }
        }
    }
}
