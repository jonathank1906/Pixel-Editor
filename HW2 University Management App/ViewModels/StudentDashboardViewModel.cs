using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using System.Linq;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using Avalonia.Controls;
using System.Collections.ObjectModel;
using System;
using System.Diagnostics;
using System.Windows.Input;

using Avalonia.Input;
namespace HW2_University_Management_App.ViewModels
{
    public partial class StudentDashboardViewModel : ViewModelBase
    {
        private readonly SubjectService subjectService = new();
        private readonly User student;
        private readonly Random random = new();

        public ObservableCollection<ColoredSubject> EnrolledSubjects { get; set; }
        public ObservableCollection<ColoredSubject> AvailableSubjects { get; set; }
        [ObservableProperty]
        private ColoredSubject selectedAvailableSubject;
        [ObservableProperty]
        private ColoredSubject selectedEnrolledSubject;

        public StudentDashboardViewModel(User student)
        {
            this.student = student;
            EnrolledSubjects = new ObservableCollection<ColoredSubject>();
            AvailableSubjects = new ObservableCollection<ColoredSubject>();

            LoadSubjects();
        }

        private void LoadSubjects()
        {
            var subjects = subjectService.GetSubjects();

            foreach (var subject in subjects)
            {
                var color = ColorStyles.GetRandomColor();

                // Compare student.EnrolledSubjects with subject.SubjectID, not subject.Name
                if (student.EnrolledSubjects.Contains(subject.SubjectID))  // Compare using SubjectID
                    EnrolledSubjects.Add(new ColoredSubject(subject.Name, color));
                else
                    AvailableSubjects.Add(new ColoredSubject(subject.Name, color));
            }
        }


        [RelayCommand]
        private void EnrollInSubject()
        {
            if (SelectedAvailableSubject != null)
            {
                student.EnrolledSubjects.Add(SelectedAvailableSubject.Name);
                subjectService.EnrollStudent(student.UserID, SelectedAvailableSubject.Name);

                EnrolledSubjects.Add(SelectedAvailableSubject);
                AvailableSubjects.Remove(SelectedAvailableSubject);
            }
        }

        [RelayCommand]
        private void DropSubject()
        {
            if (SelectedEnrolledSubject != null)
            {
                student.EnrolledSubjects.Remove(SelectedEnrolledSubject.Name);
                subjectService.DropStudent(student.UserID, SelectedEnrolledSubject.Name);

                AvailableSubjects.Add(SelectedEnrolledSubject);
                EnrolledSubjects.Remove(SelectedEnrolledSubject);
            }
        }

        [RelayCommand]
        private void OnSubjectClicked(ColoredSubject subject)
        {
            // Handle the click event for the subject
            Debug.WriteLine($"Subject clicked: {subject.Name}");
        }


    }
}
