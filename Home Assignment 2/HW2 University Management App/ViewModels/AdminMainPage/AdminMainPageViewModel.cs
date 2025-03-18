using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace HW2_University_Management_App.ViewModels.AdminMainPage
{
    public partial class AdminMainPageViewModel : ViewModelBase
    {

        private MainWindowViewModel viewchange;

        [ObservableProperty]
        private string userName;

        public AdminMainPageViewModel(MainWindowViewModel mv)
        {
            viewchange = mv;
            viewchange.window.CanResize = false;
            userName = "welcome back " + mv.userName.UserID;
        }


    }
}
