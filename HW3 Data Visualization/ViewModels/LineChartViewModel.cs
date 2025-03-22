using CommunityToolkit.Mvvm.ComponentModel; // For ObservableProperty and ViewModelBase
using LiveChartsCore; // For ISeries, Axis, and LineSeries
using LiveChartsCore.SkiaSharpView; // For SkiaSharp-based LiveCharts functionality
using LiveChartsCore.SkiaSharpView.Painting; // For SolidColorPaint
using SkiaSharp; // For SKColors (used in painting)
using System.Collections.Generic; // For List<T>
using System.Linq; // For LINQ methods like GroupBy and Select
using HW3_Data_Visualization.Models; // Assuming FoodWasteData is in this namespace
using System.Windows.Input; // For ICommand

namespace HW3_Data_Visualization.ViewModels
{
    public partial class LineChartViewModel : ViewModelBase
    {
        public ICommand? RemoveChartCommand { get; set; }

        [ObservableProperty] private string title = "Line Chart";
        [ObservableProperty] private ISeries[] seriesCollection;
        [ObservableProperty] private Axis[] xAxes;
        [ObservableProperty] private Axis[] yAxes;

        public LineChartViewModel(List<FoodWasteData> foodWasteRecords)
        {
            var groupedData = foodWasteRecords
                .GroupBy(f => f.Year)
                .Select(g => new { Year = g.Key, TotalWaste = g.Sum(f => f.TotalWaste) })
                .ToList();

            var seriesList = new[]
            {
        new LineSeries<double>
        {
            Name = "Yearly Waste",
            Values = groupedData.Select(g => g.TotalWaste).ToArray(),
            Stroke = new SolidColorPaint(SKColors.Blue, 2),
            Fill = null
        }
    };

            SeriesCollection = seriesList;

            XAxes = new[] { new Axis { Labels = groupedData.Select(g => g.Year.ToString()).ToList() } };
            YAxes = new[] { new Axis { Labeler = value => $"{value:N0} Tons", MinLimit = 0 } };
        }
    }
}