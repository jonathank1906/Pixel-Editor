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

        public ObservableCollection<ColoredSubject> EnrolledSubjects { get; set; }
        public ObservableCollection<ColoredSubject> AvailableSubjects { get; set; }

        [ObservableProperty]
        private ColoredSubject selectedAvailableSubject;

        [ObservableProperty]
        private ColoredSubject selectedEnrolledSubject;

        [ObservableProperty]
        private string searchQuery = "";

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
            EnrolledSubjects.Clear();
            AvailableSubjects.Clear();

            foreach (var subject in subjects)
            {
                var color = ColorStyles.GetRandomColor();
                var coloredSubject = new ColoredSubject(subject.SubjectID, subject.Name, subject.Description, color);

                if (student.EnrolledSubjects.Contains(subject.SubjectID))
                    EnrolledSubjects.Add(coloredSubject);
                else
                    AvailableSubjects.Add(coloredSubject);
            }
        }

        /// <summary>
        /// Filters subjects based on the search query.
        /// </summary>
        [RelayCommand]
        private void SearchSubjects()
        {
            var subjects = subjectService.GetSubjects();

            var filteredAvailable = subjects
                .Where(s => s.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) &&
                            !student.EnrolledSubjects.Contains(s.SubjectID))
                .Select(s => new ColoredSubject(s.SubjectID, s.Name, s.Description, ColorStyles.GetRandomColor()));

            var filteredEnrolled = subjects
                .Where(s => s.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) &&
                            student.EnrolledSubjects.Contains(s.SubjectID))
                .Select(s => new ColoredSubject(s.SubjectID, s.Name, s.Description, ColorStyles.GetRandomColor()));

            AvailableSubjects.Clear();
            foreach (var sub in filteredAvailable)
            {
                AvailableSubjects.Add(sub);
            }

            EnrolledSubjects.Clear();
            foreach (var sub in filteredEnrolled)
            {
                EnrolledSubjects.Add(sub);
            }
        }

        [RelayCommand]
        private void EnrollInSubject()
        {
            if (SelectedAvailableSubject != null)
            {
                student.EnrolledSubjects.Add(SelectedAvailableSubject.SubjectID);
                subjectService.EnrollStudent(student.UserID, SelectedAvailableSubject.SubjectID);

                EnrolledSubjects.Add(SelectedAvailableSubject);
                AvailableSubjects.Remove(SelectedAvailableSubject);
                subjectService.SaveData();
            }
        }

        [RelayCommand]
        private void DropSubject()
        {
            if (SelectedEnrolledSubject != null)
            {
                student.EnrolledSubjects.Remove(SelectedEnrolledSubject.SubjectID);
                subjectService.DropStudent(student.UserID, SelectedEnrolledSubject.SubjectID);

                AvailableSubjects.Add(SelectedEnrolledSubject);
                EnrolledSubjects.Remove(SelectedEnrolledSubject);
                subjectService.SaveData();
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

