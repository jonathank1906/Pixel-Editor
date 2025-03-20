using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HW2_University_Management_App.Models;
using HW2_University_Management_App.Services;
using HW2_University_Management_App.Styles;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace HW2_University_Management_App.ViewModels;
public partial class TeacherDashboardViewModel : ViewModelBase
{
    private readonly SubjectService subjectService = new();
    private readonly User teacher;

    public ObservableCollection<ColoredSubject> CreatedSubjects { get; set; }

    [ObservableProperty]
    private string newSubjectName;

    [ObservableProperty]
    private string newSubjectDescription;

    [ObservableProperty]
    private ColoredSubject selectedExistingSubject;

    [ObservableProperty]
    private string creationMessage;

    [ObservableProperty]
    private bool isEditingMode;

    [ObservableProperty]
    private string searchQuery = "";

    public TeacherDashboardViewModel(User teacher)
    {
        this.teacher = teacher;
        CreatedSubjects = new ObservableCollection<ColoredSubject>();
        LoadSubjects();
    }


    // Loads subjects from the JSON file and populates the UI list.
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
    }


    // Filters subjects based on the search query.
    [RelayCommand]
    private void SearchSubjects()
    {
        var teacherSubjects = subjectService.GetSubjects()
            .Where(s => s.TeacherID == teacher.UserID &&
                        s.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
            .Select(s => new ColoredSubject(s.SubjectID, s.Name, s.Description, ColorStyles.GetRandomColor()));

        CreatedSubjects.Clear();
        foreach (var sub in teacherSubjects)
        {
            CreatedSubjects.Add(sub);
        }
    }

    [RelayCommand]
    private void ToggleEditMode()
    {
        if (SelectedExistingSubject != null)
        {
            NewSubjectName = SelectedExistingSubject.Name;
            NewSubjectDescription = SelectedExistingSubject.Description;
            IsEditingMode = true;
        }
        else
        {
            IsEditingMode = false;
        }
    }

    [RelayCommand]
    private async Task SaveSubject()
    {
        if (SelectedExistingSubject != null && !string.IsNullOrEmpty(NewSubjectName) && !string.IsNullOrEmpty(NewSubjectDescription))
        {
            subjectService.UpdateSubject(SelectedExistingSubject.SubjectID, NewSubjectName, NewSubjectDescription);
            LoadSubjects();
            IsEditingMode = false;
            SelectedExistingSubject = null;
            CreationMessage = $"Successfully updated: {NewSubjectName}";
            NewSubjectName = "";
            NewSubjectDescription = "";
            await ClearCreationMessageAfterDelay();
        }
    }

    [RelayCommand]
    private async Task CreateSubject()
    {
        if (!string.IsNullOrEmpty(NewSubjectName) && !string.IsNullOrEmpty(NewSubjectDescription))
        {
            subjectService.CreateSubject(NewSubjectName, NewSubjectDescription, teacher.UserID);
            LoadSubjects();
            CreationMessage = $"Successfully created: {NewSubjectName}";
            NewSubjectName = "";
            NewSubjectDescription = "";
            IsEditingMode = false;
            await ClearCreationMessageAfterDelay();
        }
    }

    [RelayCommand]
    private async Task DeleteSubject()
    {
        if (SelectedExistingSubject != null)
        {
            var deletedSubject = SelectedExistingSubject;
            string subjectIdToDelete = SelectedExistingSubject.SubjectID;
            subjectService.DeleteSubject(subjectIdToDelete);
            LoadSubjects();
            SelectedExistingSubject = null;
            CreationMessage = $"Successfully deleted: {deletedSubject.Name}";
            IsEditingMode = false;
            await ClearCreationMessageAfterDelay();
        }
    }

    private async Task ClearCreationMessageAfterDelay()
    {
        await Task.Delay(3000);
        CreationMessage = string.Empty;
    }
}