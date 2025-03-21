using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using HW3_Data_Visualization.Models;
using HW3_Data_Visualization.Services;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Linq;
using CommunityToolkit.Mvvm.Input;

namespace HW3_Data_Visualization.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly CsvService _csvService;

        public ObservableCollection<BarChartViewModel> Charts { get; } = new();
        public ObservableCollection<FoodWasteData> FoodWasteRecords { get; set; } = new();

        public MainWindowViewModel()
        {
            _csvService = new CsvService();
            LoadCsvData();
        }

        private void LoadCsvData()
        {
            var filePath = "Assets/global_food_wastage_dataset.csv";  // The relative path to the CSV file
            var data = _csvService.LoadData(filePath);
            FoodWasteRecords = new ObservableCollection<FoodWasteData>(data);
        }

        [RelayCommand]
        private void ShowHouseholdWaste()
        {
            var householdData = FoodWasteRecords.Where(f => f.FoodCategory == "Household").ToList();
            Charts.Add(new BarChartViewModel(householdData)
            {
                RemoveChartCommand = RemoveChartCommand // Pass the command explicitly
            });
        }

        [RelayCommand]
        private void ShowWasteByCountry()
        {
            var groupedByCountry = FoodWasteRecords
                .GroupBy(f => f.Country)
                .Select(g => new FoodWasteData
                {
                    FoodCategory = g.Key,
                    TotalWaste = g.Sum(f => f.TotalWaste)
                })
                .ToList();
            Charts.Add(new BarChartViewModel(groupedByCountry)
            {
                RemoveChartCommand = RemoveChartCommand // Pass the command explicitly
            });
        }

        [RelayCommand]
        private void ShowYearlyWasteTrend()
        {
            var yearlyData = FoodWasteRecords
                .GroupBy(f => f.Year)
                .Select(g => new FoodWasteData
                {
                    FoodCategory = g.Key.ToString(),
                    TotalWaste = g.Sum(f => f.TotalWaste)
                })
                .ToList();
            Charts.Add(new BarChartViewModel(yearlyData)
            {
                RemoveChartCommand = RemoveChartCommand // Pass the command explicitly
            });
        }

        [RelayCommand]
        private void ShowFoodWaste()
        {
            Charts.Add(new BarChartViewModel(FoodWasteRecords.ToList())
            {
                RemoveChartCommand = RemoveChartCommand // Pass the command explicitly
            });
        }
        [RelayCommand]
        private void RemoveChart(BarChartViewModel chart)
        {
            Charts.Remove(chart);
        }
    }
}