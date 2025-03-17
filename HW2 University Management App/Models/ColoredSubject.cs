using Avalonia.Media;

public class ColoredSubject
{
    public string SubjectID { get; }
    public string Name { get; }
    public string Description { get; } // 🔹 Added Description
    public SolidColorBrush BackgroundColor { get; }

    public ColoredSubject(string id, string name, string description, SolidColorBrush color)
    {
        SubjectID = id;
        Name = name;
        Description = description;
        BackgroundColor = color;
    }
}
