using Avalonia.Media;

namespace HW2_University_Management_App.Models
{
    public class ColoredSubject
    {
        public string SubjectID { get; set; }  
        public string Name { get; set; }
        public SolidColorBrush BackgroundColor { get; set; }

        // Constructor
        public ColoredSubject(string subjectID, string name, SolidColorBrush color)
        {
            SubjectID = subjectID;  
            Name = name;
            BackgroundColor = color;
        }
    }
}
