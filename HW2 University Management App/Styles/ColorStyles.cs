using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace HW2_University_Management_App.Styles
{
    public static class ColorStyles
    {
        private static readonly List<Color> subjectColors = new()
        {
            Colors.LightBlue,
            Colors.LightGreen,
            Colors.LightCoral,  // Light red
            Colors.LightGoldenrodYellow  // Light yellow
        };

        private static readonly Random random = new();

        public static SolidColorBrush GetRandomColor()
        {
            int index = random.Next(subjectColors.Count);
            return new SolidColorBrush(subjectColors[index]);
        }
    }
}
