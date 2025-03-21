using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;

namespace HW3_Data_Visualization.ViewModels;

public partial class ChartViewModel : ViewModelBase
{
    [ObservableProperty]
    private string title = "Food Waste Chart";

    public ISeries[] SeriesCollection { get; set; }
    public Axis[] XAxes { get; set; }
    public Axis[] YAxes { get; set; }

    public ChartViewModel()
    {
        // Sample Data (Replace with actual CSV data)
        SeriesCollection = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "Food Waste (Tons)",
                Values = new double[] { 19268.63, 3916.97, 9700.16, 46299.69, 33096.57 },
                Fill = new SolidColorPaint(SKColors.Purple),
                Stroke = new SolidColorPaint(SKColors.DarkViolet) { StrokeThickness = 2 }
            }
        };

        XAxes = new Axis[]
        {
            new Axis
            {
                Labels = new List<string> { "Australia", "Indonesia", "Germany", "France", "India" },
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
