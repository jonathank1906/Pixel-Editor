using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using HW3_Data_Visualization.Models;
using System.Linq;
using System.Windows.Input;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;

namespace HW3_Data_Visualization.ViewModels
{
    public class BarChartViewModel : ChartViewModelBase
    {
        public override string Title { get; } = "Bar Chart";

        public override IEnumerable<ISeries> SeriesCollection { get; }

        public override IEnumerable<Axis> XAxes { get; }

        public override IEnumerable<Axis> YAxes { get; }

        public BarChartViewModel(IEnumerable<FoodWasteData> data)
        {
            SeriesCollection = new[]
            {
                new ColumnSeries<double>
                {
                    Values = data.Select(d => d.TotalWaste).ToArray(),
                    Name = "Waste"
                }
            };

            XAxes = new[]
            {
                new Axis { Labels = data.Select(d => d.FoodCategory).ToArray() }
            };

            YAxes = new[]
            {
                new Axis { Labeler = value => $"{value:N0} Tons" }
            };
        }
    }
}