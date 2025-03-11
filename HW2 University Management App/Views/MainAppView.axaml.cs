using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HW2_University_Management_App.Views;

public partial class MainAppView : UserControl
{
    public MainAppView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}