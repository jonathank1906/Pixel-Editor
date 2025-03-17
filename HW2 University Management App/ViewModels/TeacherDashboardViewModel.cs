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

                // Set the creation message to be displayed in the UI
                CreationMessage = $"Successfully created the subject: {newSubject.Name}";

                // Call ClearCreationMessageAfterDelay to clear the message after a delay
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

                // Delete the subject
                subjectService.DeleteSubject(deletedSubject.SubjectID);

                // Remove it from the list
                CreatedSubjects.Remove(deletedSubject);

                // Reset selection
                SelectedExistingSubject = null;

                // Set the deletion message to be displayed in the UI
                CreationMessage = $"Successfully deleted the subject: {deletedSubject.Name}";

                // Call ClearCreationMessageAfterDelay to clear the message after a delay
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
