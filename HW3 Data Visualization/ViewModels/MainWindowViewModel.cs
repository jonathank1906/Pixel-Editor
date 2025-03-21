using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using HW3_Data_Visualization.Models;
using HW3_Data_Visualization.Services;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Linq;

namespace HW3_Data_Visualization.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly CsvService _csvService;

         [ObservableProperty]
        private bool isMenuCollapsed = false; // Default is not collapsed

        public ObservableCollection<ChartViewModel> Charts { get; } = new();
        public ObservableCollection<FoodWasteData> FoodWasteRecords { get; set; } = new();

        public MainWindowViewModel()
        {
            _csvService = new CsvService();
            LoadCsvData();
            CreateChart();
        }




        // Toggle the menu
        public void ToggleMenu()
        {
            IsMenuCollapsed = !IsMenuCollapsed;
        }
        private void LoadCsvData()
        {
            var filePath = "Assets/global_food_wastage_dataset.csv";  // The relative path to the CSV file
            var data = _csvService.LoadData(filePath);
            FoodWasteRecords = new ObservableCollection<FoodWasteData>(data);
        }

        private void CreateChart()
        {
            // Create a ChartViewModel with the loaded FoodWasteRecords
            var chartViewModel = new ChartViewModel(FoodWasteRecords.ToList());

            // Add the created chart to the Charts collection
            Charts.Add(chartViewModel);
        }
        
    }
}
