using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HW3_Data_Visualization.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ChartViewModel> Charts { get; } = new();

    public MainWindowViewModel()
    {
        // Temporary chart preview — your graph teammate will replace this
        Charts.Add(new ChartViewModel
        {
            Title = "Sample Chart: Food Waste Categories"
        });
    }
}
