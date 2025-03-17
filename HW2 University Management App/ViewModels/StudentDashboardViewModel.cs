using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Media;
using System.Linq;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using System.Collections.ObjectModel;
using System;

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

        /// <summary>
        /// Loads subjects from the JSON file and assigns them to Available or Enrolled lists.
        /// </summary>
        private void LoadSubjects()
        {
            var subjects = subjectService.GetSubjects();

            foreach (var subject in subjects)
            {
                var color = ColorStyles.GetRandomColor();

                // 🔹 Ensure ColoredSubject includes the subject description
                var coloredSubject = new ColoredSubject(subject.SubjectID, subject.Name, subject.Description, color);

                if (student.EnrolledSubjects.Contains(subject.SubjectID))
                    EnrolledSubjects.Add(coloredSubject);
                else
                    AvailableSubjects.Add(coloredSubject);
            }
        }

        /// <summary>
        /// Command to enroll the student in a selected subject.
        /// </summary>
        [RelayCommand]
        private void EnrollInSubject()
        {
            if (SelectedAvailableSubject != null)
            {
                student.EnrolledSubjects.Add(SelectedAvailableSubject.SubjectID);
                subjectService.EnrollStudent(student.UserID, SelectedAvailableSubject.SubjectID);

                EnrolledSubjects.Add(SelectedAvailableSubject);
                AvailableSubjects.Remove(SelectedAvailableSubject);

                subjectService.SaveData();  // Save the data after enrollment
            }
        }

        /// <summary>
        /// Command to drop an enrolled subject.
        /// </summary>
        [RelayCommand]
        private void DropSubject()
        {
            if (SelectedEnrolledSubject != null)
            {
                student.EnrolledSubjects.Remove(SelectedEnrolledSubject.SubjectID);
                subjectService.DropStudent(student.UserID, SelectedEnrolledSubject.SubjectID);

                AvailableSubjects.Add(SelectedEnrolledSubject);
                EnrolledSubjects.Remove(SelectedEnrolledSubject);

                subjectService.SaveData();  // Save data after dropping the subject
            }
        }

        /// <summary>
        /// Handles clicking on a subject (for future features).
        /// </summary>
        [RelayCommand]
        public void OnSubjectClicked(ColoredSubject subject)
        {
            // Future implementation (e.g., show subject details)
        }

        /// <summary>
        /// Deselects any selected subject.
        /// </summary>
        public void DeselectSubject()
        {
            SelectedEnrolledSubject = null;
            SelectedAvailableSubject = null;
        }
    }
}
