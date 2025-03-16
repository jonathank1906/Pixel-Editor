namespace HW2_University_Management_App.ViewModels;
public class DialogWindowViewModel : ViewModelBase
{
    private string _message;
    
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public DialogWindowViewModel(string message)
    {
        Message = message;
    }
}
