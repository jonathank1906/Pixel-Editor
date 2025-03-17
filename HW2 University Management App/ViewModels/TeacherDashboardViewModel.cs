using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private string creationMessage; // The message to display after creating or deleting a subject

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
        private async Task CreateSubject()
        {
            if (!string.IsNullOrEmpty(NewSubjectName))
            {
                // Get correct subject ID from SubjectService
                string newSubjectId = subjectService.CreateSubject(NewSubjectName, teacher.UserID);

                // Reload subjects from the JSON file
                LoadSubjects();

                // Show success message
                CreationMessage = $"Successfully created the subject: {NewSubjectName}";

                // Reset input field
                NewSubjectName = "";

                await ClearCreationMessageAfterDelay();
            }
        }


        [RelayCommand]
        private async Task DeleteSubject()
        {
            if (SelectedExistingSubject != null)
            {
                // Store deleted subject before removing it
                var deletedSubject = SelectedExistingSubject;
                // Get selected subject ID
                string subjectIdToDelete = SelectedExistingSubject.SubjectID;

                // 🔹 Delete from JSON
                subjectService.DeleteSubject(subjectIdToDelete);

                // Reload subjects from the JSON file to ensure sync
                LoadSubjects();


                // Set the deletion message to be displayed in the UI
                CreationMessage = $"Successfully deleted the subject: {deletedSubject.Name}";

                // Reset selection
                SelectedExistingSubject = null;

                await ClearCreationMessageAfterDelay();
            }
            else
            {
                CreationMessage = string.Empty; // If no subject was selected, clear the message
            }
        }


        private async Task ClearCreationMessageAfterDelay()
        {
            await Task.Delay(3000); // Delay of 3 seconds
            CreationMessage = string.Empty; // Clear the message
        }
    }
}
