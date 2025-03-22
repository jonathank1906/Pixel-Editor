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
    public class LineChartViewModel : ChartViewModelBase
    {
        public override string Title { get; } = "Line Chart";

        public override IEnumerable<ISeries> SeriesCollection { get; }

        public override IEnumerable<Axis> XAxes { get; }

        public override IEnumerable<Axis> YAxes { get; }

        public LineChartViewModel(IEnumerable<FoodWasteData> data)
        {
            var groupedData = data
                .GroupBy(d => d.FoodCategory)
                .Select(g => new { Category = g.Key, TotalWaste = g.Sum(d => d.TotalWaste) })
                .ToList();

            SeriesCollection = new[]
            {
                new LineSeries<double>
                {
                    Values = groupedData.Select(g => g.TotalWaste).ToArray(),
                    Name = "Waste Trend"
                }
            };

            XAxes = new[]
            {
                new Axis { Labels = groupedData.Select(g => g.Category).ToArray() }
            };

            YAxes = new[]
            {
                new Axis { Labeler = value => $"{value:N0} Tons" }
            };
        }
    }
}