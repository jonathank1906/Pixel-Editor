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
        private ObservableCollection<ColoredSubject> allCreatedSubjects;
        
        [ObservableProperty]
        private string searchQuery = "";

        [ObservableProperty]
        private string newSubjectName;

        [ObservableProperty]
        private string newSubjectDescription; // 🔹 Added description field

        [ObservableProperty]
        private ColoredSubject selectedExistingSubject; // The subject selected for deletion

        [ObservableProperty]
        private string creationMessage; // The message to display after creating or deleting a subject

        [ObservableProperty]
        private bool isEditingMode;

        public TeacherDashboardViewModel(User teacher)
        {
            this.teacher = teacher;
            CreatedSubjects = new ObservableCollection<ColoredSubject>();
            allCreatedSubjects = new ObservableCollection<ColoredSubject>();

            LoadSubjects();
        }

        /// <summary>
        /// Loads subjects from the JSON file and populates the UI list.
        /// </summary>
        private void LoadSubjects()
        {
            CreatedSubjects.Clear();

            var teacherSubjects = subjectService.GetSubjects()
                .Where(s => s.TeacherID == teacher.UserID);

            foreach (var subject in teacherSubjects)
            {
                var color = ColorStyles.GetRandomColor();
                CreatedSubjects.Add(new ColoredSubject(subject.SubjectID, subject.Name, subject.Description, color));
            }
             UpdateFilteredSubjects();
        }

        /// <summary>
        /// Toggles between Create and Edit mode.
        /// </summary>
        /// 
        [RelayCommand]

         private void UpdateFilteredSubjects()
        {
            CreatedSubjects.Clear();
            foreach (var subject in allCreatedSubjects.Where(s => s.Name.Contains(SearchQuery, System.StringComparison.OrdinalIgnoreCase)))
                CreatedSubjects.Add(subject);
        }
        [RelayCommand]
        private void ToggleEditMode()
        {
            if (SelectedExistingSubject != null)
            {
                // 🔹 Load selected subject details into input fields
                NewSubjectName = SelectedExistingSubject.Name;
                NewSubjectDescription = SelectedExistingSubject.Description;
                IsEditingMode = true;
            }
            else
            {
                IsEditingMode = false;
            }
        }

        /// <summary>
        /// Saves the updated subject details.
        /// </summary>
        [RelayCommand]
        private async Task SaveSubject()
        {
            if (SelectedExistingSubject != null && !string.IsNullOrEmpty(NewSubjectName) && !string.IsNullOrEmpty(NewSubjectDescription))
            {
                // 🔹 Update the existing subject
                subjectService.UpdateSubject(SelectedExistingSubject.SubjectID, NewSubjectName, NewSubjectDescription);

                // Reload subjects to reflect changes
                LoadSubjects();

                // Reset editing state
                IsEditingMode = false;
                SelectedExistingSubject = null;

                // Show success message
                CreationMessage = $"Successfully updated: {NewSubjectName}";

                // Reset input fields
                NewSubjectName = "";
                NewSubjectDescription = "";

                await ClearCreationMessageAfterDelay();
            }
        }

        /// <summary>
        /// Command to create a new subject with name & description.
        /// </summary>
        [RelayCommand]
        private async Task CreateSubject()
        {
            if (!string.IsNullOrEmpty(NewSubjectName) && !string.IsNullOrEmpty(NewSubjectDescription))
            {
                // 🔹 Create the subject with description
                string newSubjectId = subjectService.CreateSubject(NewSubjectName, NewSubjectDescription, teacher.UserID);

                // Reload subjects from JSON to reflect changes
                LoadSubjects();

                // Show success message
                CreationMessage = $"Successfully created: {NewSubjectName}";
                // Reset input fields
                NewSubjectName = "";
                NewSubjectDescription = "";

                // Reset editing state
                IsEditingMode = false;

                await ClearCreationMessageAfterDelay();
            }
        }

        /// <summary>
        /// Command to delete the selected subject.
        /// </summary>
        [RelayCommand]
        private async Task DeleteSubject()
        {
            if (SelectedExistingSubject != null)
            {
                var deletedSubject = SelectedExistingSubject;
                // Get selected subject ID
                string subjectIdToDelete = SelectedExistingSubject.SubjectID;

                // 🔹 Delete from JSON
                subjectService.DeleteSubject(subjectIdToDelete);

                // Reload subjects from JSON to ensure sync
                LoadSubjects();

                // Reset selection
                SelectedExistingSubject = null;

                // Set the deletion message to be displayed in the UI
                CreationMessage = $"Successfully deleted the subject: {deletedSubject.Name}";

                // Reset editing state
                IsEditingMode = false;

                await ClearCreationMessageAfterDelay();
            }
            else
            {
                CreationMessage = string.Empty; // If no subject was selected, clear the message
            }
        }

        /// <summary>
        /// Clears the creation message after 3 seconds.
        /// </summary>
        private async Task ClearCreationMessageAfterDelay()
        {
            await Task.Delay(3000); // Delay of 3 seconds
            CreationMessage = string.Empty; // Clear the message
        }
    }
}