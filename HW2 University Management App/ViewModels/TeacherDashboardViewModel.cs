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
        private ColoredSubject selectedCreatedSubject; // Corrected to use ColoredSubject type

        public TeacherDashboardViewModel(User teacher)
        {
            this.teacher = teacher;
            CreatedSubjects = new ObservableCollection<ColoredSubject>();

            LoadSubjects();
        }

        private void LoadSubjects()
        {
            CreatedSubjects.Clear();
            // Filter subjects by the teacher's UserID
            var teacherSubjects = subjectService.GetSubjects().Where(s => s.TeacherID == teacher.UserID);

            foreach (var subject in teacherSubjects)
            {
                var color = ColorStyles.GetRandomColor();
                // Create ColoredSubject with SubjectID, Name, and BackgroundColor
                CreatedSubjects.Add(new ColoredSubject(subject.SubjectID, subject.Name, color)); // Ensure SubjectID is passed
            }
        }

        [RelayCommand]
        private void CreateSubject()
        {
            if (!string.IsNullOrEmpty(NewSubjectName))
            {
                // Create new subject and pass teacher's UserID
                subjectService.CreateSubject(NewSubjectName, teacher.UserID);
                LoadSubjects(); // Refresh subjects with the newly created subject
                NewSubjectName = ""; // Reset input field
            }
        }

        [RelayCommand]
        private void DeleteSubject()
        {
            if (SelectedCreatedSubject != null)  // Check if a subject is selected
            {
                // Delete subject by its SubjectID (use the correct identifier)
                subjectService.DeleteSubject(SelectedCreatedSubject.SubjectID); 
                LoadSubjects(); // Refresh the list after deletion
            }
        }
    }
}
