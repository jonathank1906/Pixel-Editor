using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using HW3_Data_Visualization.Models;
using HW3_Data_Visualization.Services;

namespace HW3_Data_Visualization.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly CsvService _csvService;

    public ObservableCollection<ChartViewModel> Charts { get; } = new();
    public ObservableCollection<FoodWasteData> FoodWasteRecords { get; set; } = new();

    public MainWindowViewModel()
    {
        _csvService = new CsvService();
        LoadCsvData();
          var chartViewModel = new ChartViewModel();
    Charts.Add(chartViewModel);
    }

    private void LoadCsvData()
    {
        var filePath = "Assets/global_food_wastage_dataset.csv";  // Update this path
        var data = _csvService.LoadData(filePath);
        FoodWasteRecords = new ObservableCollection<FoodWasteData>(data);
    }
}
