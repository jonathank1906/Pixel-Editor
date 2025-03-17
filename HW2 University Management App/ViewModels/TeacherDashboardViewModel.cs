﻿using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using System.Linq;
using System;

namespace HW2_University_Management_App.ViewModels
{
    public partial class TeacherDashboardViewModel : ViewModelBase
    {
        private readonly SubjectService subjectService = new();
        private readonly User teacher;

        public ObservableCollection<ColoredSubject> CreatedSubjects { get; set; }

        [ObservableProperty]
        private string newSubjectName;

        [ObservableProperty]
        private ColoredSubject selectedExistingSubject; // The subject selected for deletion

        [ObservableProperty]
        private ColoredSubject newlyCreatedSubject; // The newly created subject


        public TeacherDashboardViewModel(User teacher)
        {
            this.teacher = teacher;
            CreatedSubjects = new ObservableCollection<ColoredSubject>();

            LoadSubjects();
        }

        private void LoadSubjects()
        {
           CreatedSubjects.Clear();

            var teacherSubjects = subjectService.GetSubjects().Where(s => s.TeacherID == teacher.UserID);
            foreach (var subject in teacherSubjects)
            {
                var color = ColorStyles.GetRandomColor();
                CreatedSubjects.Add(new ColoredSubject(subject.SubjectID, subject.Name, color));
            }
        }

       [RelayCommand]
private void CreateSubject()
{
    if (!string.IsNullOrEmpty(NewSubjectName))
    {
        // Call CreateSubject and assume it creates the subject without returning the ID
        subjectService.CreateSubject(NewSubjectName, teacher.UserID);

        // Generate a new subject ID, or assume the backend assigns it
        var newSubjectId = Guid.NewGuid().ToString(); // For example, you can use a GUID if the backend doesn't return an ID

        // Create the ColoredSubject with a random background color
        var newSubject = new ColoredSubject(newSubjectId, NewSubjectName, ColorStyles.GetRandomColor());

        // Add the new subject to the CreatedSubjects collection
        CreatedSubjects.Add(newSubject);

        // Reset the input field for new subject name
        NewSubjectName = "";
        
        // Set the newly created subject for displaying the success message
        NewlyCreatedSubject = newSubject; // Show this as the selected subject for the dialog

        // Optionally, call LoadSubjects() to refresh the list from the service
        // LoadSubjects(); // This can be omitted if you want the UI to reflect the change immediately without reloading
    }
}




        [RelayCommand]
        private void DeleteSubject()
        {
            if (SelectedExistingSubject != null)
            {
                // Store deleted subject before removing it
                NewlyCreatedSubject = SelectedExistingSubject;

                // Delete the subject
                subjectService.DeleteSubject(SelectedExistingSubject.SubjectID);

                // Remove it from the list
                CreatedSubjects.Remove(SelectedExistingSubject);

                // Reset selection
                SelectedExistingSubject = null;
            }
            else
            {
                NewlyCreatedSubject = null; // Avoid old values affecting the message
            }
        }
    }
}
