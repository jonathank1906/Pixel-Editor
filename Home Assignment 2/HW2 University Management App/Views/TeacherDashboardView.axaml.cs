using Avalonia.Controls;
using Avalonia.Interactivity;
using HW2_University_Management_App.ViewModels;

namespace HW2_University_Management_App.Views
{
    public partial class TeacherDashboardView : UserControl
    {
        private TeacherDashboardViewModel ViewModel => (TeacherDashboardViewModel)DataContext;

        public TeacherDashboardView()
        {
            InitializeComponent();
        }
    }
}
