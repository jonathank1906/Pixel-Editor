using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace HW2_University_Management_App.Styles
{
    public static class ColorStyles
    {
        private static readonly List<Color> subjectColors = new()
        {
            Color.Parse("#FFD5BC"),
            Color.Parse("#D7CCC8"),
            Color.Parse("#DACAF6"),
            Color.Parse("#F8C7DE"),
            Color.Parse("#C4ECFF"),
            Color.Parse("#C5F0C9"),
            Color.Parse("#E9F6CC"),
            Color.Parse("#FEEBBB"),
            Color.Parse("#D9E3E8"),
        };

        private static readonly Random random = new();

        public static SolidColorBrush GetRandomColor()
        {
            int index = random.Next(subjectColors.Count);
            return new SolidColorBrush(subjectColors[index]);
        }
    }
}
