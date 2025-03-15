using Avalonia.Media;

namespace HW2_University_Management_App.Models
{
    public class ColoredSubject
    {
        public string Name { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }

        public ColoredSubject(string name, SolidColorBrush color)
        {
            Name = name;
            BackgroundColor = color;
        }
    }
}
