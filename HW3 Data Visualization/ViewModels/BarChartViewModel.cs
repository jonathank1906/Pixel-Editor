using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using HW3_Data_Visualization.Models;
using System.Linq;
using System.Windows.Input;

namespace HW3_Data_Visualization.ViewModels
{
    public partial class BarChartViewModel : ViewModelBase
    {
        public ICommand? RemoveChartCommand { get; set; }

        [ObservableProperty]
        private string title = "Food Waste Chart";

        [ObservableProperty]
        private ISeries[] seriesCollection;

        [ObservableProperty]
        private Axis[] xAxes;

        [ObservableProperty]
        private Axis[] yAxes;

        // Constructor to initialize the data from FoodWasteData
        public BarChartViewModel(List<FoodWasteData> foodWasteRecords)
        {
            // Create column series for each food category dynamically
            var groupedData = foodWasteRecords
                .GroupBy(f => f.FoodCategory)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalWaste = g.Sum(f => f.TotalWaste)
                })
                .ToList();

            var seriesList = new List<ISeries>();
            foreach (var group in groupedData)
            {
                var series = new ColumnSeries<double>
                {
                    Name = group.Category,
                    Values = new double[] { group.TotalWaste },
                    Fill = new SolidColorPaint(SKColors.Orange)
                };
                seriesList.Add(series);
            }

            // Set the SeriesCollection to the dynamically generated data
            SeriesCollection = seriesList.ToArray();

            // Set up the X and Y axes
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = groupedData.Select(g => g.Category).ToList(),
                    LabelsRotation = 15
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Labeler = value => $"{value:N0} Tons",
                    MinLimit = 0
                }
            };
        }
    }
}
